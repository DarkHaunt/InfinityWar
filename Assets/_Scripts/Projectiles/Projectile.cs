using System.Collections;
using System;
using InfinityGame.GameEntities;
using UnityEngine;

namespace InfinityGame.Projectiles
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        public event Action OnExpluatationEnd;
        protected event Action<Transform> OnHeadingTowardsTarget;

        [SerializeField] protected float _damage = 10f;
        [SerializeField] protected float _speedMult = 2f;

        [SerializeField] protected Rigidbody2D _rigidbody2D;

        [Range(1f, 10f)]
        [SerializeField] private float _lifeTime = 5f;

        [SerializeField] private string _shooterFractionTag;

        protected ObjectDispatcher _disptacher; // Implements fly trajectory

        private Coroutine _lifeTimeCoroutine;


        protected abstract void OnCollitionWith(FractionEntity target);

        public void PullInPreparations()
        {
            StopCoroutine(_lifeTimeCoroutine);
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            gameObject.SetActive(true);
            _lifeTimeCoroutine =  StartCoroutine(LifeTimeCoroutine());
        }

        protected void EndExpluatation() => OnExpluatationEnd?.Invoke();

        public void HeadTowardsTarget(Transform target)
        {
            _disptacher.DispatchProjectileToTarget(_rigidbody2D, target);

            OnHeadingTowardsTarget?.Invoke(target);
        }

        protected bool IsColliderEnemyEntity(Collider2D collider2D, out FractionEntity enemy)
        {
            var isHitableEntity = collider2D.TryGetComponent(out FractionEntity entity);

            enemy = entity;
            return isHitableEntity && !entity.IsSameFraction(_shooterFractionTag);
        }

        private IEnumerator LifeTimeCoroutine()
        {
            yield return new WaitForSeconds(_lifeTime);

            OnExpluatationEnd?.Invoke();
        }
        

        protected virtual void Awake()
        {
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsColliderEnemyEntity(collision, out FractionEntity enemy))
                OnCollitionWith(enemy);
        }

    } 
}
