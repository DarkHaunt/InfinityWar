using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Warrior : FractionEntity
{
    private const float MinimalDistanceToAttack = 0.5f;
    public static event Action<Warrior> OnMainTargetSwitch;

    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackCoolDown;

    [SerializeField] protected float _speedMult;

    [Range(MinimalDistanceToAttack, 30)]
    [SerializeField] protected float _attackDistance;

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private ArguingTrigger _arguingTrigger;

    [SerializeField] protected FractionEntity _globalTarget = null; // Target, which will be constantly followed by this warrior
    [SerializeField] protected FractionEntity _localTarget = null;  // Target around, which was deceted by arguing trigger

    [SerializeField] protected WarriorState _currentState = WarriorState.FollowGlobalTarget;

    // Baked variables
    private WaitForSeconds _waitForSecondsAttackCooldown;

    private bool _isOnCoolDown = false;
    [SerializeField] private bool _isCanArgueOnAreaTarget = true;


    private FractionEntity LocalTarget
    {
        set
        {
            _localTarget = value;

            if (_localTarget is null)
            {
                _isCanArgueOnAreaTarget = true;
                _currentState = WarriorState.FollowGlobalTarget;
                return;
            }

            _isCanArgueOnAreaTarget = false;
            _currentState = WarriorState.Arguing;
        }
    }


    protected abstract void Attack();

    public void OnStateUpdate()
    {
        switch (_currentState)
        {
            case WarriorState.Attack:
                TryToAttack();
                break;
            case WarriorState.Arguing:
                CheckForAttackDistance();
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

        Attack();
        StartCoroutine(CoolDownCoroutine());
    }

    private IEnumerator CoolDownCoroutine()
    {
        _isOnCoolDown = true;

        yield return _waitForSecondsAttackCooldown;

        _isOnCoolDown = false;
    }

    private void CheckForAttackDistance()
    {
        var walkDirection = (_localTarget.transform.position - transform.position).normalized;
        _rigidbody2D.velocity = walkDirection * _speedMult;

        var distanceForEnemy = Vector3.Distance(_localTarget.transform.position, transform.position);

        if (distanceForEnemy <= _attackDistance)
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

    public void SetNewGlobalTarget(FractionEntity globalTarget) => _globalTarget = globalTarget;

    private void GetNewGlobalTarget() => OnMainTargetSwitch?.Invoke(this);


    protected virtual void Awake()
    {
        _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);

        // TODO: Всё таки переделать на подписку метода у локальной цели на ивент OnDie
        _arguingTrigger.OnEntityEnter += (FractionEntity target) =>
        {
            if (_isCanArgueOnAreaTarget && !target.IsSameFraction(FractionTag))
                LocalTarget = target; // If warrioir hasn't target before - chose it and start arguing
        };

        _arguingTrigger.OnEntityExit += (FractionEntity target) =>
        {
            if (_localTarget == target)
                TryToGetNewLocalTarget();
        };

        GetNewGlobalTarget();
        TryToGetNewLocalTarget();
        _globalTarget.OnDie += GetNewGlobalTarget;
    }

    protected virtual void Update() => OnStateUpdate();

    protected override void OnDestroy()
    {
        base.OnDestroy();

        _globalTarget.OnDie -= GetNewGlobalTarget;
    }


    protected enum WarriorState
    {
        Attack,
        Arguing,
        FollowGlobalTarget,
    }
}
