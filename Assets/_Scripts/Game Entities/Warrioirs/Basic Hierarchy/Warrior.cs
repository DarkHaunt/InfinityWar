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



        protected GameEntity LocalTarget => _localTarget;
        public string PoolTag => _poolTag;



        protected abstract void Attack();

        public virtual void PullInPreparations()
        {
            _globalTarget.OnDie -= GetNewGlobalTarget;

            _entityDetector.OnEntityEnter -= OnEntityDetected;
            _entityDetector.OnEntityExit -= OnEntityLose;

            gameObject.SetActive(false);

            SetLocalTarget(null);
            _globalTarget = null;
        }

        public virtual void PullOutPreparation()
        {
            _isOnCoolDown = false;

            gameObject.SetActive(true);
            SubscribeForDetector();
        }

        public void Init(Warrior prefab, Vector2 position)
        {
            Init(prefab.Fraction, prefab.Health);
            transform.position = position;
            StartCoroutine(TryToGetLocalTargetCoroutine());

            GetNewGlobalTarget();
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
            var enemiesAround = GetEnemiesAround();
            var newLocalTarget = GameEntitiesDetector.GetClosestEntityToPosition(transform.position, enemiesAround);

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
            var enemyEntities = FractionCacher.GetEnemyEntitiesOfFraction(Fraction);
            _globalTarget = GameEntitiesDetector.GetClosestEntityToPosition(transform.position, enemyEntities);

            if (_globalTarget != null)
                _globalTarget.OnDie += GetNewGlobalTarget;
            else
                BecomeNeutral();
        }

        /// <summary>
        /// Won't be able to argue / attack anything anymore
        /// </summary>
        private void BecomeNeutral()
        {
            _entityDetector.gameObject.SetActive(false);
            _currentState = WarriorState.Unactive;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private IEnumerable<GameEntity> GetEnemiesAround()
        {
            foreach (var entity in _entityDetector.DetectedEntities)
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
        private void SubscribeForDetector()
        {
            _entityDetector.OnEntityEnter += OnEntityDetected;
            _entityDetector.OnEntityExit += OnEntityLose;
        }

        private IEnumerator TryToGetLocalTargetCoroutine()
        {
            yield return new WaitForFixedUpdate();

            TryToGetNewLocalTarget();
        }

        private void OnEntityDetected(GameEntity target)
        {
            if (!_isOnArgue && !target.IsBelongsToFraction(Fraction))
                SetLocalTarget(target);
        }

        private void OnEntityLose(GameEntity target)
        {
            if (_localTarget == target)
                TryToGetNewLocalTarget();
        }



        protected virtual void Awake()
        {
            _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);

            SubscribeForDetector();
            StartCoroutine(TryToGetLocalTargetCoroutine());
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
