using System.Collections.Generic;
using System.Collections;
using System;
using InfinityGame.Strategies.ProjectileCollisionAction;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Projectile : MonoBehaviour, IPoolable
    {
        public event Action OnExploitationEnd;

        [Header("--- Projectile Settings ---")]
        [SerializeField] private string _poolTag;

        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _speedMult = 2f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Range(1f, 10f)]
        [SerializeField] private float _lifeTime = 5f;

        [SerializeField] private List<ProjectileEntityCollisionAction> _behaviors;

        private readonly ObjectRotator _objectRotator = new ObjectRotator();

        // Cached data
        private Coroutine _lifeTimeCoroutine;
        private WaitForSeconds _cachedLifeTime;
        private string _fractionTag;

        private bool _isExploitating = true;



        public string FractionTag => _fractionTag;
        public string PoolTag => _poolTag;
        public float Damage => _damage;
        protected bool IsExploitating => _isExploitating;



        public void PullInPreparations()
        {
            StopCoroutine(_lifeTimeCoroutine);
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            _isExploitating = true;
            gameObject.SetActive(true);
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        protected virtual void OnCollisionWith(GameEntity target)
        {
            target.GetDamage(Damage);

            foreach (var behavior in _behaviors)
                behavior.OnCollisionBehave(this, target);
        }

        public void SetFlyDirection(Vector2 direction)
        {
            _rigidbody2D.velocity = direction * _speedMult;

            _objectRotator.RoteteObjectToTarget(_rigidbody2D, direction);
        }

        public void SetFractionTag(string fractionTag)
        {
            _fractionTag = fractionTag;
        }

        private IEnumerator LifeTimeCoroutine()
        {
            yield return _cachedLifeTime;

            EndExploitation();
        }

        protected void EndExploitation()
        {
            _isExploitating = false;
            OnExploitationEnd?.Invoke();
        }

        protected void RestartLifeTime()
        {
            StopCoroutine(_lifeTimeCoroutine);
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        private bool IsColliderEnemyEntity(Collider2D collider2D, out GameEntity enemy)
        {
            var isHitableEntity = collider2D.TryGetComponent(out GameEntity entity);

            enemy = entity;
            return isHitableEntity && !entity.IsBelongsToFraction(FractionTag);
        }



        protected virtual void Awake()
        {
            _cachedLifeTime = new WaitForSeconds(_lifeTime);

            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        protected virtual void OnDestroy() { }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsColliderEnemyEntity(collision, out GameEntity enemy) && _isExploitating)
                OnCollisionWith(enemy);
        }
    }
}
