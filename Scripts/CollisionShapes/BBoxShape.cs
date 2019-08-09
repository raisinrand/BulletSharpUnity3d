﻿using BulletSharp;
using UnityEngine;

namespace BulletUnity
{
    [AddComponentMenu("Physics Bullet/Shapes/Box")]
    public class BBoxShape : BCollisionShape
    {
        //public Vector3 extents = Vector3.one;

        [SerializeField]
        protected Vector3 extents = Vector3.one;
        public Vector3 Extents
        {
            get { return extents; }
            set
            {
                if (collisionShapePtr != null && value != extents)
                {
                    Debug.LogError("Cannot change the extents after the bullet shape has been created. Extents is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else
                {
                    extents = value;
                }
            }
        }
        //final scale passed to bullet
        public Vector3 FinalScaling => Vector3.Scale(LocalScaling,transform.lossyScale);

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
            {
                return;
            }
            Vector3 position = transform.position;
            Quaternion rotation = transform.rotation;
            Vector3 scale = FinalScaling;
            BUtility.DebugDrawBox(position, rotation, scale, extents, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            BoxShape bs = new BoxShape(extents.ToBullet());
            bs.LocalScaling = FinalScaling.ToBullet();
            bs.Margin = m_Margin;
            return bs;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new BoxShape(extents.ToBullet());
                ((BoxShape)collisionShapePtr).LocalScaling = FinalScaling.ToBullet();
                collisionShapePtr.Margin = m_Margin;
            }
            return collisionShapePtr;
        }
    }
}
