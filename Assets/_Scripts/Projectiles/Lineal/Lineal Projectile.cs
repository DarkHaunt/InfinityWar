using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class LinealProjectile : Projectile
{
    protected override abstract void OnTargetTouch(FractionEntity target);

    public override void Throw(Transform targetTransform)
    {
        _rigidbody2D.velocity = (targetTransform.position - transform.position).normalized * _speedMult;
    }
}
