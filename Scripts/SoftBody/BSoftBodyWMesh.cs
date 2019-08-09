using UnityEngine;
using System.Collections;
using BulletSharp.SoftBody;
using System;
using BulletSharp;
using System.Collections.Generic;
//using BulletSharp.SoftBody;

namespace BulletUnity
{

    /// <summary>
    /// Used base for any(most) softbodies needing a mesh and meshrenderer.
    /// </summary>
    //[RequireComponent(typeof(MeshFilter))]
    //[RequireComponent(typeof(MeshRenderer))]
    public class BSoftBodyWMesh : BSoftBody
    {
        public BUserMeshSettings meshSettings = new BUserMeshSettings();

        [Tooltip("Anchors are Bullet rigid bodies that some soft body nodes/vertices have been bound to. Vertex colors in the Soft Body mesh are used " +
                " to map the nodes/vertices to the anchors. The red channel defines the strength of the anchor. The green channel defines which anchor a" +
                " vertex will be bound to.")]
        public BAnchor[] anchors;

        private MeshFilter _meshFilter;
        protected MeshFilter meshFilter
        {
            get { return _meshFilter = _meshFilter ?? GetComponent<MeshFilter>(); }
        }
        private Mesh mesh;

        public override void Start()
        {
            mesh = meshSettings.Build();
            BindNodesToAnchors();
            base.Start();
        }
        public void BindNodesToAnchors()
        {
            Vector3[] verts = mesh.vertices;
            Vector3[] norms = mesh.normals;
            Color[] cols = mesh.colors;
            int[] triangles = mesh.triangles;
            // clear old values
            for (int j = 0; j < anchors.Length; j++)
            {
                anchors[j].anchorNodeIndexes.Clear();
                anchors[j].anchorNodeStrength.Clear();
                anchors[j].anchorPosition.Clear();
            }

            int numAnchorNodes = 0;
            for (int i = 0; i < cols.Length; i++)
            {
                for (int j = 0; j < anchors.Length; j++)
                {
                    if (cols[i].g > anchors[j].colRangeFrom &&
                        cols[i].g <= anchors[j].colRangeTo)
                    {
                        anchors[j].anchorNodeIndexes.Add(i);
                        anchors[j].anchorNodeStrength.Add(cols[i].r);
                        anchors[j].anchorPosition.Add(verts[i]);
                        numAnchorNodes++;
                    }
                }
            }
        }
        internal override bool _BuildCollisionObject()
        {
            mesh = meshSettings.Build();
            if (mesh == null)
            {
                Debug.LogError("Could not build mesh from meshSettings for " + this);
                return false;
            }

            GetComponent<MeshFilter>().sharedMesh = mesh;

            if (World == null)
            {
                return false;
            }
            //convert the mesh data to Bullet data and create SoftBody
            BulletSharp.Math.Vector3[] bVerts = new BulletSharp.Math.Vector3[mesh.vertexCount];
            Vector3[] verts = mesh.vertices;
            for (int i = 0; i < mesh.vertexCount; i++)
            {
                bVerts[i] = verts[i].ToBullet();
            }

            SoftBody m_BSoftBody = SoftBodyHelpers.CreateFromTriMesh(World.WorldInfo, bVerts, mesh.triangles);
            m_collisionObject = m_BSoftBody;

            m_BSoftBody.Scale(transform.localScale.ToBullet());
            
            SoftBodySettings.ConfigureSoftBody(m_BSoftBody);         //Set SB settings

            //Set SB position to GO position
            m_BSoftBody.Rotate(transform.rotation.ToBullet());
            m_BSoftBody.Translate(transform.position.ToBullet());
            
            for (int i = 0; i < anchors.Length; i++)
            {
                BAnchor a = anchors[i];
                for (int j = 0; j < a.anchorNodeIndexes.Count; j++)
                {
                    m_BSoftBody.AppendAnchor(a.anchorNodeIndexes[j], (RigidBody) a.anchorRigidBody.GetCollisionObject(), false, a.anchorNodeStrength[j]);
                }
            }

            return true;
        }

        /// <summary>
        /// Create new SoftBody object using a Mesh
        /// </summary>
        /// <param name="position">World position</param>
        /// <param name="rotation">rotation</param>
        /// <param name="mesh">Need to provide a mesh</param>
        /// <param name="buildNow">Build now or configure properties and call BuildSoftBody() after</param>
        /// <param name="sBpresetSelect">Use a particular softBody configuration pre select values</param>
        /// <returns></returns>
        public static GameObject CreateNew(Vector3 position, Quaternion rotation, Mesh mesh, bool buildNow, SBSettingsPresets sBpresetSelect)
        {
            GameObject go = new GameObject("SoftBodyWMesh");
            go.transform.position = position;
            go.transform.rotation = rotation;
            BSoftBodyWMesh BSoft = go.AddComponent<BSoftBodyWMesh>();
            MeshFilter meshFilter = go.AddComponent<MeshFilter>();
            MeshRenderer meshRenderer = go.AddComponent<MeshRenderer>();
            BSoft.meshSettings.UserMesh = mesh;
            UnityEngine.Material material = new UnityEngine.Material(Shader.Find("Standard"));
            meshRenderer.material = material;

            BSoft.SoftBodySettings.ResetToSoftBodyPresets(sBpresetSelect); //Apply SoftBody settings presets

            if (buildNow)
            {
                BSoft._BuildCollisionObject();  //Build the SoftBody
            }
            go.name = "BSoftBodyWMesh";
            return go;
        }

        /// <summary>
        /// Update Mesh (or line renderer) at runtime, call from Update 
        /// </summary>
        public override void UpdateMesh()
        {
            mesh.vertices = verts;
            mesh.normals = norms;
            mesh.RecalculateBounds();
            transform.SetTransformationFromBulletMatrix(m_collisionObject.WorldTransform);  //Set SoftBody position, No motionstate    
        }
    }
}