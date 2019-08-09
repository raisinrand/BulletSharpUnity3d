using System;
using System.Collections.Generic;
using UnityEngine;
using BulletUnity;


namespace BulletUnity
{
    [Serializable]
    public class BAnchor
    {
        [Tooltip("A Bullet Physics rigid body")]
        public BRigidBody anchorRigidBody;
        [Tooltip("A range in the green channel. Vertices with a vertex color green value in this range will be bound to this anchor")]
        public float colRangeFrom = 0f;
        [Tooltip("A range in the green channel. Vertices with a vertex color green value in this range will be bound to this anchor")]
        public float colRangeTo = 1f;
        [HideInInspector]
        public List<int> anchorNodeIndexes = new List<int>();
        [HideInInspector]
        public List<float> anchorNodeStrength = new List<float>();
        [HideInInspector]
        public List<Vector3> anchorPosition = new List<Vector3>();
    }
}