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


        protected override void Awake()
        {
            base.Awake();

            OnHeadingTowardsTarget += (Transform target) => _rotateStrategy.RoteteObjectToTarget(_rigidbody2D, target);
        }
    }
}
