using System;
using System.Collections;
using System.Collections.Generic;
using InfinityGame.CashedData;
using UnityEngine;

namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Warrior : FractionEntity, IPoolable
    {
        private const float MinimalDistanceToAttack = 0.5f;

        [SerializeField] private float _attackCoolDown;

        [SerializeField] private float _speedMult;

        [Range(MinimalDistanceToAttack, 30)]
        [SerializeField] private float _attackDistance;

        [SerializeField] private Rigidbody2D _rigidbody2D; // TODO : Убрать не нужные сериализации (После глубоких тестов перед отправкой)
        [SerializeField] private EntityDetector _entityDetector;

        [SerializeField] private FractionEntity _globalTarget = null; // Target, which will be constantly followed by this warrior
        [SerializeField] private FractionEntity _localTarget = null; // Target around, which was deceted by arguing trigger

        [SerializeField] private string _poolTag;

        [SerializeField] private WarriorState _currentState = WarriorState.FollowGlobalTarget;

        private bool _isOnCoolDown = false;
        private bool _isOnArgue = true;

        // Baked variables
        private WaitForSeconds _waitForSecondsAttackCooldown;
        private float _maxHealthPoints;



        protected FractionEntity LocalTarget => _localTarget;
        public string PoolTag => _poolTag;



        protected abstract void Attack();

        public void PullInPreparations()
        {
            _globalTarget.OnZeroHealth -= GetNewGlobalTarget;

            FractionCasher.OnGameEnd -= BecomeNeutral;
            gameObject.SetActive(false);
        }

        public void PullOutPreparation()
        {
            _health = _maxHealthPoints;
            GetNewGlobalTarget();

            FractionCasher.OnGameEnd += BecomeNeutral;
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
                case WarriorState.Stay:
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

            Attack();
            StartCoroutine(CoolDownCoroutine());
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
            var enumenatorOfEnemies = enemiesAround.GetEnumerator();

            // No need to allocate momory for variables below, if there is no any enemies
            if (!enumenatorOfEnemies.MoveNext())
            {
                SetLocalTarget(null);
                return;
            }

            FractionEntity newLocaltarget = enumenatorOfEnemies.Current;
            var minimalDiscoveredDistance = float.MaxValue;

            foreach (var enemy in enemiesAround)
            {
                var distanceToCurrentTarget = Vector3.Distance(enemy.transform.position, newLocaltarget.transform.position);

                if (distanceToCurrentTarget < minimalDiscoveredDistance)
                {
                    newLocaltarget = enemy;
                    minimalDiscoveredDistance = distanceToCurrentTarget;
                }
            }

            SetLocalTarget(newLocaltarget);
        }

        private void SetLocalTarget(FractionEntity newLocalTarget)
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
            var minimalDistanceToTwonHall = float.MaxValue;
            //var cashedBuildings = BuildingCasher.GetCashedBuildings();
            var cashedBuildings = FractionCasher.GetEnemyEntities(FractionTag);

            foreach (var building in cashedBuildings)
            {
          /*      if (building.IsSameFraction(FractionTag))
                    continue;*/

                var distanceToCurrentTownhall = Vector3.Distance(building.transform.position, transform.position);

                if (distanceToCurrentTownhall < minimalDistanceToTwonHall)
                {
                    _globalTarget = building;
                    minimalDistanceToTwonHall = distanceToCurrentTownhall;
                }
            }

            _globalTarget.OnZeroHealth += GetNewGlobalTarget;
        }

        /// <summary>
        /// Won't be able to argue / attack anything anymore
        /// </summary>
        private void BecomeNeutral()
        {
            _globalTarget.OnZeroHealth -= GetNewGlobalTarget;

            _entityDetector.gameObject.SetActive(false);
            _currentState = WarriorState.Stay;
            _rigidbody2D.velocity = Vector2.zero;
        }

        private IEnumerable<FractionEntity> GetEnemiesAround()
        {
            foreach (var entity in _entityDetector.DetecedEntities)
                if (!entity.IsSameFraction(FractionTag))
                    yield return entity;
        }

        private IEnumerator CoolDownCoroutine()
        {
            _isOnCoolDown = true;

            yield return _waitForSecondsAttackCooldown;

            _isOnCoolDown = false;
        }




        protected virtual void Awake()
        {
            _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);

            _entityDetector.OnEntityEnter += (FractionEntity target) =>
            {
                if (!_isOnArgue && !target.IsSameFraction(FractionTag))
                    SetLocalTarget(target);
            };

            _entityDetector.OnEntityExit += (FractionEntity target) =>
            {
                if (_localTarget == target)
                    TryToGetNewLocalTarget();
            };

            FractionCasher.OnGameEnd += BecomeNeutral;

            _maxHealthPoints = _health;
        }

        private void Start()
        {
            GetNewGlobalTarget();
            TryToGetNewLocalTarget();
        }

        protected virtual void Update() => OnStateUpdate();



        protected enum WarriorState
        {
            Attack,
            Arguing,
            FollowGlobalTarget,
            Stay
        }
    }
}
