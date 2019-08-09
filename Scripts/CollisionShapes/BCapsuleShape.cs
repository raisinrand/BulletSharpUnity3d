using BulletSharp;
using UnityEngine;

namespace BulletUnity
{

    [AddComponentMenu("Physics Bullet/Shapes/Capsule")]
    public class BCapsuleShape : BCollisionShape
    {
        public enum CapsuleAxis
        {
            x,
            y,
            z
        }

        //input radius
        [SerializeField]
        protected float radius = 1f;
        public float Radius
        {
            get { return radius; }
            set
            {
                if (collisionShapePtr != null && value != radius)
                {
                    Debug.LogError("Cannot change the radius after the bullet shape has been created. Radius is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else
                {
                    radius = value;
                }
            }
        }
        //radius with scaling applied
        public float ScaledRadius
        {
            get
            {
                return Radius*FinalScaling[(((int)upAxis) + 2) % 3];
            }
        }

        //input height
        [SerializeField]
        protected float height = 2f;
        public float Height
        {
            get { return height; }
            set
            {
                if (collisionShapePtr != null && value != height)
                {
                    Debug.LogError("Cannot change the height after the bullet shape has been created. Height is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else
                {
                    height = value;
                }
            }
        }
        //height with scaling applied
        public float ScaledHeight
        {
            get
            {
                 return Height*FinalScaling[(int)upAxis];
            }
        }

        [SerializeField]
        protected CapsuleAxis upAxis = CapsuleAxis.y;
        public CapsuleAxis UpAxis
        {
            get { return upAxis; }
            set
            {
                if (collisionShapePtr != null && value != upAxis)
                {
                    Debug.LogError("Cannot change the upAxis after the bullet shape has been created. upAxis is only the initial value " +
                                    "Use LocalScaling to change the shape of a bullet shape.");
                }
                else
                {
                    upAxis = value;
                }
            }
        }

        //final scale passed to bullet
        public Vector3 FinalScaling
        {
            get{
                Vector3 scale = Vector3.Scale(LocalScaling,transform.lossyScale);
                // scale[(int)upAxis] *= 1 + (2*radius/height);
                return scale;
            }
        }

        public override void OnDrawGizmosSelected()
        {
            if (drawGizmo == false)
            {
                return;
            }
            UnityEngine.Vector3 position = transform.position;
            UnityEngine.Quaternion rotation = transform.rotation;

            BUtility.DebugDrawCapsule(position, rotation, FinalScaling, radius, height*0.5f, (int)upAxis, Gizmos.color);
        }

        CapsuleShape _CreateCapsuleShape()
        {
            CapsuleShape cs = null;
            if (upAxis == CapsuleAxis.x)
            {
                cs = new CapsuleShapeX(radius, height);
            }
            else if (upAxis == CapsuleAxis.y)
            {
                cs = new CapsuleShape(radius, height);
            }
            else if (upAxis == CapsuleAxis.z)
            {
                cs = new CapsuleShapeZ(radius, height);
            }
            else
            {
                Debug.LogError("invalid axis value");
            }
            cs.LocalScaling = FinalScaling.ToBullet();
            cs.Margin = m_Margin;
            return cs;
        }

        public override CollisionShape CopyCollisionShape()
        {
            return _CreateCapsuleShape();
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = _CreateCapsuleShape();
            }
            return collisionShapePtr;
        }
    }
}
