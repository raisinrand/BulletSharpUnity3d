using BulletSharp;
using UnityEngine;

namespace BulletUnity
{
    [AddComponentMenu("Physics Bullet/Shapes/Sphere")]
    public class BSphereShape : BCollisionShape
    {
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
        //final scale passed to bullet
        public Vector3 FinalScaling => Vector3.Scale(LocalScaling,transform.lossyScale);

        public override void OnDrawGizmosSelected()
        {
            if (!drawGizmo)
                return;
                
            UnityEngine.Vector3 position = transform.position;
            UnityEngine.Quaternion rotation = transform.rotation;
            BUtility.DebugDrawSphere(position, rotation, FinalScaling, Vector3.one * radius, Color.yellow);
        }

        public override CollisionShape CopyCollisionShape()
        {
            SphereShape ss = new SphereShape(radius);
            ss.LocalScaling = FinalScaling.ToBullet();
            return ss;
        }

        public override CollisionShape GetCollisionShape()
        {
            if (collisionShapePtr == null)
            {
                collisionShapePtr = new SphereShape(radius);
                ((SphereShape)collisionShapePtr).LocalScaling = FinalScaling.ToBullet();
            }
            return collisionShapePtr;
        }
    }
}
