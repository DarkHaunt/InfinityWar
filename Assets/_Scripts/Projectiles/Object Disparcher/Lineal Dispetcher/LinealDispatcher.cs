using InfinityGame.Projectiles;
using UnityEngine;

/// <summary>
/// Makes object move for a static line
/// </summary>
public class LinealDispatcher : ObjectDispatcher
{
    public LinealDispatcher(float throwSpeed) : base(throwSpeed) { }

    public override void DispatchProjectileToTarget(Rigidbody2D objectRigidbody2D, Transform target)
    {
        var moveDirection = (target.position - objectRigidbody2D.transform.position).normalized;
        objectRigidbody2D.velocity = moveDirection * _speedMult;
    }
}
