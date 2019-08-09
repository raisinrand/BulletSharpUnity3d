using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletSharp;

namespace BulletUnity
{
    public class BAction : IAction
    {
        System.Action<CollisionWorld, float> updateAction;
        System.Action<IDebugDraw> debugDraw;
        public BAction(System.Action<CollisionWorld, float> updateAction, System.Action<IDebugDraw> debugDraw = null)
        {
            this.updateAction = updateAction;
            this.debugDraw = debugDraw;
        }
        public void DebugDraw(IDebugDraw debugDrawer)
        {
            debugDraw(debugDrawer);
        }
        public void UpdateAction(CollisionWorld collisionWorld, float deltaTimeStep)
        {
            updateAction(collisionWorld, deltaTimeStep);
        }
    }
}
