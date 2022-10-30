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
        protected event Action<Transform> OnHeadingTowardsTargetStart;

        [SerializeField] private string _shooterFractionTag;
        [SerializeField] private string _poolTag;

        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _speedMult = 2f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Range(1f, 10f)]
        [SerializeField] private float _lifeTime = 5f;

        private Coroutine _lifeTimeCoroutine;
        private ObjectDispatcher _disptacher; // Implements fly trajectory



        public string FractionTag => _shooterFractionTag;
        public string PoolTag => _poolTag;
        protected float Damage => _damage;
        protected float Speed => _speedMult;
        protected Rigidbody2D RigidBody2D => _rigidbody2D;



        protected abstract void OnCollisionWith(FractionEntity target);

        public void PullInPreparations()
        {
            StopCoroutine(_lifeTimeCoroutine);
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            gameObject.SetActive(true);
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        public void HeadTowardsTarget(Transform target)
        {
            OnHeadingTowardsTargetStart?.Invoke(target);

            _disptacher.DispatchProjectileToTarget(_rigidbody2D, target);
        }

        protected void InitializeDispatcher(ObjectDispatcher dispatcher)
        {
            _disptacher = dispatcher;
        }

        private IEnumerator LifeTimeCoroutine()
        {
            yield return new WaitForSeconds(_lifeTime);

            EndExpluatation();
        }

        protected void EndExpluatation()
        {
            OnExpluatationEnd?.Invoke();
        }

        private bool IsColliderEnemyEntity(Collider2D collider2D, out FractionEntity enemy)
        {
            var isHitableEntity = collider2D.TryGetComponent(out FractionEntity entity);

            enemy = entity;
            return isHitableEntity && !entity.IsBelongToFraction(_shooterFractionTag);
        }



        protected virtual void Awake()
        {
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        protected virtual void OnDestroy() { }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsColliderEnemyEntity(collision, out FractionEntity enemy))
                OnCollisionWith(enemy);
        }
    }
}
