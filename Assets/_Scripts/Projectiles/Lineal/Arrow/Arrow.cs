using UnityEngine;

public class Arrow : LinealProjectile, IRotatable
{
    protected override void OnTargetTouch(FractionEntity target)
    {
        target.GetDamage(_damage);

        Destroy(gameObject);
    }

    public override void Throw(Transform targetTransform)
    {
        RoteteToTarget(targetTransform);

        base.Throw(targetTransform);
    }

    public void RoteteToTarget(Transform target)
    {
        var xDistance = target.position.x - transform.position.x;
        var yDistance = target.position.y - transform.position.y;

        _rigidbody2D.rotation = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg - 90f;
    }
}


