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

        [SerializeField] private List<ProjectileColliisionBehavior> _behaviors;

        private readonly ObjectRotator _objectRotator = new ObjectRotator();

        // Cached data
        private Coroutine _lifeTimeCoroutine;
        private WaitForEndOfFrame _cachedWaitForFrame;
        private string _fractionTag;

        private float _currentLifeTime = 0f;

        private bool _isExploitating = true;



        public string FractionTag => _fractionTag;
        public string PoolTag => _poolTag;
        public float Speed => _speedMult;
        public float LifeTime => _currentLifeTime;
        protected bool IsExploitating => _isExploitating;



        public void PullInPreparations()
        {
            StopLifeTime();
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            _isExploitating = true;
            gameObject.SetActive(true);
            StartLifeTimeCounting();
        }

        protected virtual void OnCollisionWith(GameEntity target)
        {
            foreach (var behavior in _behaviors)
                behavior.OnCollisionBehave(target, this);
        }

        public void SetFlyDirection(Vector2 direction)
        {
            _rigidbody2D.velocity = direction * Speed;

            _objectRotator.RoteteObjectToTarget(_rigidbody2D, direction);
        }

        public void SetFractionTag(string fractionTag)
        {
            _fractionTag = fractionTag;
        }

        private IEnumerator LifeTimeCoroutine()
        {
            while(_currentLifeTime < _maxLifeTime)
            {
                yield return _cachedWaitForFrame;

                _currentLifeTime += Time.deltaTime;
            }

            EndExploitation();
        }

        protected void EndExploitation()
        {
            _isExploitating = false;
            OnExploitationEnd?.Invoke();
        }

        private bool IsColliderEnemyEntity(Collider2D collider2D, out GameEntity enemy)
        {
            var isHitableEntity = collider2D.TryGetComponent(out GameEntity entity);

            enemy = entity;
            return isHitableEntity && !entity.IsBelongsToFraction(FractionTag);
        }

        protected void RestartLifeTime()
        {
            StopLifeTime();
            StartLifeTimeCounting();
        }

        private void StopLifeTime()
        {
            _currentLifeTime = 0f;
            StopCoroutine(_lifeTimeCoroutine);
        }

        private void StartLifeTimeCounting()
        {
            _lifeTimeCoroutine = StartCoroutine(LifeTimeCoroutine());
        }



        protected virtual void Awake()
        {
            _cachedWaitForFrame = new WaitForEndOfFrame();
            StartLifeTimeCounting();
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (IsColliderEnemyEntity(collision, out GameEntity enemy) && _isExploitating)
                OnCollisionWith(enemy);
        }
    }
}
