using System.Collections.Generic;
using System.Collections;
using System;
using InfinityGame.Strategies.ProjectileCollisionBehaviors;
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

        [SerializeField] private float _speedMult = 2f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Range(1f, 10f)]
        [SerializeField] private float _maxLifeTime = 5f;

        [SerializeField] private List<ProjectileColliisionBehavior> _collisionBehaviors;

        private float _currentLifeTime = 0f;
        private bool _isExploitating = true;

        // Cached data
        private Coroutine _lifeTimeCoroutine;
        private WaitForEndOfFrame _cachedWaitForFrame;
        private string _fractionTag;



        public string FractionTag => _fractionTag;
        public string PoolTag => _poolTag;
        public float Speed => _speedMult;
        public float LifeTime => _currentLifeTime;
        protected bool IsExploitating => _isExploitating;



        public void PullInPreparations()
        {
            EndLifeTime();
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            _isExploitating = true;
            gameObject.SetActive(true);
            StartLifeTime();
        }

        protected virtual void OnCollisionWith(GameEntity target)
        {
            foreach (var behavior in _collisionBehaviors)
                if (!target.IsDead)
                    behavior.OnCollisionBehave(target, this);
        }

        protected void EndExploitation()
        {
            _isExploitating = false;
            OnExploitationEnd?.Invoke();
        }

        public void SetFlyDirection(Vector2 direction)
        {
            _rigidbody2D.velocity = direction * Speed;

            ObjectRotator.RoteteYLocalAxisOnDirection(_rigidbody2D, direction);
        }

        protected void RestartLifeTime()
        {
            EndLifeTime();
            StartLifeTime();
        }

        private void EndLifeTime()
        {
            StopCoroutine(_lifeTimeCoroutine);
            _currentLifeTime = 0f;
        }

        private void StartLifeTime()
        {
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }

        private IEnumerator LifeTimeCoroutine()
        {
            while (_currentLifeTime < _maxLifeTime)
            {
                yield return _cachedWaitForFrame;

                _currentLifeTime += Time.deltaTime;
            }

            EndExploitation();
        }

        public void SetFractionTag(string fractionTag)
        {
            _fractionTag = fractionTag;
        }

        private bool IsColliderHasEnemyEntity(Collider2D collider2D, out GameEntity enemyEntity)
        {
            var isHitableEntity = collider2D.TryGetComponent(out GameEntity entity);

            enemyEntity = entity;
            return isHitableEntity && !entity.IsBelongsToFraction(FractionTag);
        }



        protected virtual void Awake()
        {
            _cachedWaitForFrame = new WaitForEndOfFrame();
            StartLifeTime();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsColliderHasEnemyEntity(collision, out GameEntity enemyEntity) && _isExploitating)
                OnCollisionWith(enemyEntity);
        }
    }
}
