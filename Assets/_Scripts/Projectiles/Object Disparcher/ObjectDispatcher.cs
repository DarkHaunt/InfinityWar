using System.Collections;
using System.Collections.Generic;
using InfinityGame.Projectiles;
using UnityEngine;

public abstract class ObjectDispatcher
{
    protected readonly float _speedMult;

    protected ObjectDispatcher(float throwSpeed)
    {
        _speedMult = throwSpeed;
    }

    public abstract void DispatchProjectileToTarget(Rigidbody2D objectRigidbody2D, Transform target);
}
