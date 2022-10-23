using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.Projectiles
{
    /// <summary>
    /// Projectile, which has straight line traectory of moving
    /// </summary>
    public abstract class RotatableProjectile : Projectile
    {
        protected IObjectRotateStrategy _rotateStrategy;


        public override void HeadTowardsTarget(Transform target)
        {
            base.HeadTowardsTarget(target);

            _rotateStrategy.RoteteObjectToTarget(RigidBody2D, target);
        }
    }
}
