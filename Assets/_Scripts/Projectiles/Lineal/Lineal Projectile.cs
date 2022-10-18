using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.Projectiles
{
    /// <summary>
    /// Projectile, which has straight line traectory of moving
    /// </summary>
    public abstract class LinealProjectile : Projectile
    {
        public override void ThrowToTarget(Transform targetTransform)
        {
            _rigidbody2D.velocity = (targetTransform.position - transform.position).normalized * _speedMult;
        }
    }
}
