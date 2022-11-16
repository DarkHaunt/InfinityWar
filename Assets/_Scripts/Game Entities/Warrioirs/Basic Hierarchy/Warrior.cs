using System.Collections;
using System.Collections.Generic;
using InfinityGame.DataCaching;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Warrior : GameEntity, IPoolable
    {
        private const float MinimalAttackDistance = 0.5f;

        [Header("--- Attack Parameters ---")]
        [Range(0f, 10f)]
        [SerializeField] private float _attackCoolDown;

        [Range(0f, 10f)]
        [SerializeField] private float _speedMult;

        [Range(MinimalAttackDistance, 30f)]
        [SerializeField] private float _attackDistance;

        [Header("--- Other ---")]
        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private EntityDetector _entityDetector;

        [Header("--- Pooling ---")]
        [SerializeField] private string _poolTag;

        [SerializeField] private GameEntity _globalTarget = null; // Target, which will be constantly followed by this warrior
        [SerializeField] private GameEntity _localTarget = null; // Target around, which was deceted by detector 

        private WarriorState _currentState = WarriorState.FollowGlobalTarget;

        private bool _isOnCoolDown = false;
        private bool _isOnArgue = true;

        // Baked variables
        private WaitForSeconds _waitForSecondsAttackCooldown;
        private float _maxHealthPoints;



        protected GameEntity LocalTarget => _localTarget;
        public string PoolTag => _poolTag;



        protected abstract void Attack();

        public void PullInPreparations()
        {
            _globalTarget.OnDie -= GetNewGlobalTarget;

            GameInitializer.OnGameEnd -= BecomeNeutral;
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            _health = _maxHealthPoints;
            _isOnCoolDown = false;
            _isDead = false;

            GetNewGlobalTarget();

            GameInitializer.OnGameEnd += BecomeNeutral;
            gameObject.SetActive(true);
        }

        private void OnStateUpdate()
        {
            switch (_currentState)
            {
                case WarriorState.Attack:
                    TryToAttack();
                    break;
                case WarriorState.Arguing:
                    FollowLocalTarget();
                    break;
                case WarriorState.FollowGlobalTarget:
                    FollowGlobalTarget();
                    break;
                case WarriorState.Unactive:
                    break;
                default:
                    throw new UnityException("Warrioir's state machine can't find current state!");
            }
        }

        private void TryToAttack()
        {
            if (_isOnCoolDown)
                return;

            if (!IsOnAttackDistance())
                _currentState = WarriorState.Arguing;

            StartCoroutine(CoolDownCoroutine());
            Attack();
        }

        private bool IsOnAttackDistance() => Vector3.Distance(_localTarget.transform.position, transform.position) < _attackDistance;

        private void FollowLocalTarget()
        {
            var walkDirection = (_localTarget.transform.position - transform.position).normalized;
            _rigidbody2D.velocity = walkDirection * _speedMult;

            if (IsOnAttackDistance())
            {
                _currentState = WarriorState.Attack;
                _rigidbody2D.velocity = Vector2.zero;
            }
        }

        private void FollowGlobalTarget()
        {
            var walkDirection = (_globalTarget.transform.position - transform.position).normalized;

            _rigidbody2D.velocity = walkDirection * _speedMult;
        }

        private void TryToGetNewLocalTarget()
        {
            var newLocalTarget = GameEntitiesDetector.GetClosestEntity(transform.position, GetEnemiesAround());

            SetLocalTarget(newLocalTarget);
        }

        private void SetLocalTarget(GameEntity newLocalTarget)
        {
            _localTarget = newLocalTarget;

            if (_localTarget is null)
            {
                _isOnArgue = false;
                _currentState = WarriorState.FollowGlobalTarget;
                return;
            }

            _isOnArgue = true;
            _currentState = WarriorState.Arguing;
        }

        private void GetNewGlobalTarget()
        {
            _globalTarget = GameEntitiesDetector.GetClosestEntity(transform.position, FractionCacher.GetEnemyEntitiesOfFraction(Fraction));

            _globalTarget.OnDie += GetNewGlobalTarget;
        }

        /// <summary>
        /// Won't be able to argue / attack anything anymore
        /// </summary>
        private void BecomeNeutral()
        {
            _globalTarget.OnDie -= GetNewGlobalTarget;

            _entityDetector.gameObject.SetActive(false);
            _currentState = WarriorState.Unactive;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private IEnumerable<GameEntity> GetEnemiesAround()
        {
            foreach (var entity in _entityDetector.DetecedEntities)
                if (!entity.IsBelongsToFraction(Fraction))
                    yield return entity;
        }

        private IEnumerator CoolDownCoroutine()
        {
            _isOnCoolDown = true;

            yield return _waitForSecondsAttackCooldown;

            _isOnCoolDown = false;
        }

        /// <summary>
        /// Waits when detector will detect all frame entities around, to get local target properly
        /// </summary>
        /// <returns></returns>
        private IEnumerator SubscribeForDetector()
        {
            yield return new WaitForFixedUpdate();

            _entityDetector.OnEntityEnter += (GameEntity target) =>
            {
                if (!_isOnArgue && !target.IsBelongsToFraction(Fraction))
                    SetLocalTarget(target);
            };

            _entityDetector.OnEntityExit += (GameEntity target) =>
            {
                if (_localTarget == target)
                    TryToGetNewLocalTarget();
            };

            GameInitializer.OnGameEnd += BecomeNeutral;
            TryToGetNewLocalTarget();
        }




        protected virtual void Awake()
        {
            _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);

            _maxHealthPoints = _health;
        }

        private void Start()
        {
            StartCoroutine(SubscribeForDetector());
            GetNewGlobalTarget();
        }

        protected virtual void Update() => OnStateUpdate();



        protected enum WarriorState
        {
            Attack,
            Arguing,
            FollowGlobalTarget,
            Unactive
        }
    }
}
