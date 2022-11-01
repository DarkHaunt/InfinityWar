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
        private IObjectRotateStrategy _rotateStrategy;



        protected void InitializeRotationStrategy(IObjectRotateStrategy rotateStrategy)
        {
            _rotateStrategy = rotateStrategy;
        }

        private void RotateToTarget(Transform target) => _rotateStrategy.RoteteObjectToTarget(RigidBody2D, target);



        protected override void Awake()
        {
            OnHeadingTowardsTarget += RotateToTarget;

            base.Awake();
        }

        protected override void OnDestroy()
        {
            OnHeadingTowardsTarget -= RotateToTarget;

            base.OnDestroy();
        }
    }
}
