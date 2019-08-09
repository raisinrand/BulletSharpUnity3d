﻿using BulletSharp;
using System;
using UnityEngine;

namespace BulletUnity
{

    [AddComponentMenu("Physics Bullet/Shapes/Multi Sphere")]
    public class BMultiSphereShape : BCollisionShape
    {
        [Serializable]
        public struct Sphere
        {
            public Vector3 position;
            public float radius;
        }

        public Sphere[] spheres = new Sphere[0];

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
            {
                return;
            }
            for (int i = 0; i < spheres.Length; i++)
            {
                Vector3 v = spheres[i].position;
                v = Vector3.Scale(v,BulletScaling);
                BUtility.DebugDrawSphere(transform.TransformPoint(v), Quaternion.identity, Vector3.one, Vector3.one * spheres[i].radius, Gizmos.color);
            }
        }

        MultiSphereShape _CreateMultiSphereShape()
        {
            BulletSharp.Math.Vector3[] positions = new BulletSharp.Math.Vector3[spheres.Length];
            float[] radius = new float[spheres.Length];
            for (int i = 0; i < spheres.Length; i++)
            {
                positions[i] = spheres[i].position.ToBullet();
                radius[i] = spheres[i].radius;
            }
            MultiSphereShape mss = new MultiSphereShape(positions, radius);
            mss.LocalScaling = EffectiveScaling.ToBullet();
            mss.Margin = m_Margin;
            return mss;
        }

        public override CollisionShape CopyCollisionShape()
        {
            return _CreateMultiSphereShape();
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = _CreateMultiSphereShape();
            }
            return collisionShapePtr;
        }
    }
}
