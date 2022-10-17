using System.Collections;
using UnityEngine;


[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected float _damage = 10f;
    [SerializeField] protected float _speedMult = 2f;
    [SerializeField] protected Rigidbody2D _rigidbody2D;

    [Range(1f, 10f)]
    [SerializeField] private float _lifeTime = 5f;

    [SerializeField] private string _shooterFractionTag;


    public abstract void Throw(Transform targetTransform);
    protected abstract void OnTargetTouch(FractionEntity target);

    protected bool ColliderIsEnemyEntity(Collider2D collider2D, out FractionEntity enemy)
    {
        var isHitableEntity = collider2D.TryGetComponent(out FractionEntity entity);

        enemy = entity;
        return isHitableEntity && !entity.IsSameFraction(_shooterFractionTag);
    }

    private IEnumerator LifeTimeCoroutine()
    {
        yield return new WaitForSeconds(_lifeTime);

        Destroy(gameObject);
    }


    private void Awake()
    {
        StartCoroutine(LifeTimeCoroutine());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (ColliderIsEnemyEntity(collision, out FractionEntity enemy))
            OnTargetTouch(enemy);
    }
}
