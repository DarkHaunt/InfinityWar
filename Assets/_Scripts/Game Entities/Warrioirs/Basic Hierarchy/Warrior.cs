using System.Collections;
using System.Collections.Generic;
using InfinityGame.DataCaching;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    public abstract class Warrior : GameEntity, IPoolable
    {
        private const float MinimalAttackDistance = 0.5f;
        private const float LocalTargetUpdateTime = 0.5f;

        private static readonly WaitForSeconds WaitForLocalTargetUpdate = new WaitForSeconds(LocalTargetUpdateTime);


        [Header("--- Attack Parameters ---")]
        [Range(0f, 10f)]
        [SerializeField] private float _attackCoolDown;

        [Range(0f, 10f)]
        [SerializeField] private float _speedMult;

        [SerializeField] private float _agressionRadius;

        [Range(MinimalAttackDistance, 30f)]
        [SerializeField] private float _attackDistance;

        [Header("--- Other ---")]
        [SerializeField] private Rigidbody2D _rigidbody2D;

        [Header("--- Pooling ---")]
        [SerializeField] private string _poolTag;

        [SerializeField] private GameEntity _globalTarget = null; // Target, which will be constantly followed by this warrior
        [SerializeField] private GameEntity _localTarget = null; // Target around, which was deceted by detector 

        private WarriorState _currentState = WarriorState.FollowGlobalTarget;

        // Flags
        private bool _isOnAttackCoolDown = false;
        private bool _isOnLocalTargetWaiting = false;

        // Baked variables
        private WaitForSeconds _waitForSecondsAttackCooldown;



        protected GameEntity LocalTarget => _localTarget;
        public string PoolTag => _poolTag;



        protected abstract void Attack();

        public virtual void PullInPreparations()
        {
            _globalTarget.OnDie -= GetNewGlobalTarget;

            if (_localTarget != null)
                _localTarget.OnDie -= OnLocalTargetDeath;

            gameObject.SetActive(false);
            ResetTargets();
        }

        public virtual void PullOutPreparation()
        {
            _isOnAttackCoolDown = false;

            gameObject.SetActive(true);
            _currentState = WarriorState.FollowGlobalTarget;
        }

        public void Init(Warrior prefab, Vector2 position)
        {
            Init(prefab.Fraction, prefab.Health);
            transform.position = position;

            GetNewGlobalTarget();
            GetNewLocalTarget();
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
                    CheckForLocalTarget();
                    break;
                case WarriorState.Unactive:
                    break;
                default:
                    throw new UnityException("Warrioir's state machine can't find current state!");
            }
        }

        private void TryToAttack()
        {
            if (_isOnAttackCoolDown)
                return;

            if (!IsOnAttackDistance())
                _currentState = WarriorState.Arguing;

            StartCoroutine(AttackCoolDownCoroutine());
            Attack();
        }

        private bool IsOnAttackDistance() => Vector3.Distance(_localTarget.transform.position, transform.position) < _attackDistance;

        private void FollowLocalTarget()
        {
            FollowTarget(_localTarget);

            if (IsOnAttackDistance())
            {
                _currentState = WarriorState.Attack;
                _rigidbody2D.velocity = Vector2.zero;
            }
        }

        private void FollowGlobalTarget() => FollowTarget(_globalTarget);

        private void FollowTarget(GameEntity target)
        {
            var walkDirection = (target.transform.position - transform.position).normalized;

            _rigidbody2D.velocity = walkDirection * _speedMult;
        }

        private void GetNewGlobalTarget()
        {
            var cachedEnemyEntities = FractionCacher.GetEnemyEntitiesOfFraction(Fraction);

            _globalTarget = EntitiesDetector.TryToGetClosestEntityToPosition(transform.position, cachedEnemyEntities);

            if (_globalTarget != null)
                _globalTarget.OnDie += GetNewGlobalTarget;
            else
                BecomeUnactive();
        }

        private void CheckForLocalTarget()
        {
            if (_isOnLocalTargetWaiting)
                return;

            StartCoroutine(CheckForLocalTargetCoroutine());
        }

        private void GetNewLocalTarget()
        {
            var newLocalTarget = EntitiesDetector.TryToGetClosestEntityToPosition(transform.position, _agressionRadius, Fraction);

            SetLocalTarget(newLocalTarget);
        }

        private void SetLocalTarget(GameEntity newLocalTarget)
        {
            _localTarget = newLocalTarget;

            if (_localTarget is null)
            {
                _isOnLocalTargetWaiting = false; 
                return;
            }

            _localTarget.OnDie += OnLocalTargetDeath;
            _currentState = WarriorState.Arguing;
        }

        private void OnLocalTargetDeath()
        {
            GetNewLocalTarget();

            if (_localTarget is null)
                _currentState = WarriorState.FollowGlobalTarget;
        }

        /// <summary>
        /// Won't be able to argue on / attack anything anymore
        /// </summary>
        private void BecomeUnactive()
        {
            ResetTargets();
            _currentState = WarriorState.Unactive;
            _rigidbody2D.velocity = Vector2.zero;
        }

        /// <summary>
        /// Resets global and local warrior targets
        /// </summary>
        private void ResetTargets()
        {
            SetLocalTarget(null);
            _globalTarget = null;
        }

        private IEnumerator CheckForLocalTargetCoroutine()
        {
            _isOnLocalTargetWaiting = true;

            yield return WaitForLocalTargetUpdate;

            GetNewLocalTarget();
        }

        private IEnumerator AttackCoolDownCoroutine()
        {
            _isOnAttackCoolDown = true;

            yield return _waitForSecondsAttackCooldown;

            _isOnAttackCoolDown = false;
        }



        protected virtual void Awake()
        {
            _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);
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
