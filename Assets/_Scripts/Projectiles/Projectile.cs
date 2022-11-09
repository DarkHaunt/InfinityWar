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
        public event Action OnExploitationEnd;

        [SerializeField] private string _poolTag;

        [SerializeField] private float _damage = 10f;
        [SerializeField] private float _speedMult = 2f;

        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Range(1f, 10f)]
        [SerializeField] private float _lifeTime = 5f;

        private Coroutine _lifeTimeCoroutine;

        private WaitForSeconds _cachedLifeTime;
        private bool _isExploitating = true;

        private string _fractionTag;



        public string FractionTag => _fractionTag;
        public string PoolTag => _poolTag;
        protected float Damage => _damage;
        protected Rigidbody2D RigidBody2D => _rigidbody2D;



        protected abstract void OnCollisionWith(GameEntity target);

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

        public virtual void SetFlyDirection(Vector2 direction)
        {
            _rigidbody2D.velocity = direction * _speedMult;
        }

        public void SetFractionTag(string fractionTag) => _fractionTag = fractionTag;

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
