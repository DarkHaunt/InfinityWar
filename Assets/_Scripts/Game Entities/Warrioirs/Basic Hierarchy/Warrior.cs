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
    public abstract class Warrior : FractionEntity
    {
        private const float MinimalDistanceToAttack = 0.5f;

        [SerializeField] private float _attackCoolDown;

        [SerializeField] private float _speedMult;

        [Range(MinimalDistanceToAttack, 30)]
        [SerializeField] private float _attackDistance;

        [SerializeField] private Rigidbody2D _rigidbody2D;
        [SerializeField] private ArguingTrigger _arguingTrigger;

        [SerializeField] private FractionEntity _globalTarget = null; // Target, which will be constantly followed by this warrior
        [SerializeField] protected FractionEntity _localTarget = null; // Target around, which was deceted by arguing trigger

        private WarriorState _currentState = WarriorState.FollowGlobalTarget;

        // Baked variables
        private WaitForSeconds _waitForSecondsAttackCooldown;

        private bool _isOnCoolDown = false;
        private bool _isOnArgue = true;


        private FractionEntity LocalTarget
        {
            set
            {
                _localTarget = value;

                if (_localTarget is null)
                {
                    _isOnArgue = false;
                    _currentState = WarriorState.FollowGlobalTarget;
                    return;
                }

                _localTarget.OnDie += TryToGetNewLocalTarget;
                _isOnArgue = true;
                _currentState = WarriorState.Arguing;
            }
        }


        protected abstract void Attack();

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

        private IEnumerator CoolDownCoroutine()
        {
            _isOnCoolDown = true;

            yield return _waitForSecondsAttackCooldown;

            _isOnCoolDown = false;
        }

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
                LocalTarget = null;
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

            LocalTarget = newLocaltarget;
        }

        private IEnumerable<FractionEntity> GetEnemiesAround()
        {
            foreach (var entity in _arguingTrigger.GetEntitiesInTriggerArea)
                if (!entity.IsSameFraction(FractionTag))
                    yield return entity;
        }

        private void GetNewGlobalTarget()
        {
            var minimalDistanceToTwonHall = float.MaxValue;
            var cashedBuilding = GameCasher.GetCashedBuildings();

            foreach (var townHall in cashedBuilding)
            {
                if (townHall.IsSameFraction(FractionTag))
                    continue;

                var distanceToCurrentTownhall = Vector3.Distance(townHall.transform.position, transform.position);

                if (distanceToCurrentTownhall < minimalDistanceToTwonHall)
                {
                    _globalTarget = townHall;
                    minimalDistanceToTwonHall = distanceToCurrentTownhall;
                }
            }

            _globalTarget.OnDie += GetNewGlobalTarget;
        }


        protected virtual void Awake()
        {
            _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);

            _arguingTrigger.OnEntityEnter += (FractionEntity target) =>
            {
                if (!_isOnArgue && !target.IsSameFraction(FractionTag))
                    LocalTarget = target;
            };

            _arguingTrigger.OnEntityExit += (FractionEntity target) =>
            {
                if (_localTarget == target)
                    TryToGetNewLocalTarget();
            };
        }

        private void Start()
        {
            GetNewGlobalTarget();
            TryToGetNewLocalTarget();
        }

        protected virtual void Update() => OnStateUpdate();

        private void OnDestroy()
        {
            _globalTarget.OnDie -= GetNewGlobalTarget;
        }


        protected enum WarriorState
        {
            Attack,
            Arguing,
            FollowGlobalTarget,
        }
    }
}
