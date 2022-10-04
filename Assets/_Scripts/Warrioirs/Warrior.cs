using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public abstract class Warrior : HitableEntity
{
    [SerializeField] protected float _damage;
    [SerializeField] protected float _attackCoolDown;

    [SerializeField] protected float _speedMult;

    [SerializeField] protected float _attackDistance;

    [SerializeField] private Rigidbody2D _rigidbody2D;
    [SerializeField] private ArguingTrigger _arguingTrigger; // Trigger, for arguing

    protected HitableEntity _globalTarget; // If warrioir haven't targets for him around, he will be chasing it until it dies, or until warrioir dies instead.
    [SerializeField] protected HitableEntity _currentTarget; // Current attacked target by this warrior
    protected WarriorState _currentState;


    private HitableEntity CurrentTarget
    {
        set
        {
            _currentTarget = value;

            if(_currentTarget is null)
            {
                _isCanArgueOnAreaTarget = true;
                _currentState = WarriorState.FollowGlobalTarget;
            }
            else
            {
                _isCanArgueOnAreaTarget = false;
                _currentState = WarriorState.Arguing;
            }
        }
    }

    // Baked variables
    private WaitForSeconds _waitForSecondsAttackCooldown; 

    private bool _isOnCoolDown = false;
    private bool _isCanArgueOnAreaTarget = true;


    protected abstract void Attack();
    protected abstract bool IsOnArguingDistance();


    public void OnStateUpdate()
    {
        switch (_currentState)
        {
            case WarriorState.Attack:
                if (!_isOnCoolDown)
                {
                    Attack();
                    SetCooldown();
                }
                break;
            case WarriorState.Arguing:
                CheckForAttackDistance();
                break;
            case WarriorState.FollowGlobalTarget:
                FollowGlobalTarget();
                break;
            case WarriorState.Cooldown:
                break;
            default:
                throw new UnityException("Warrioir's state machine can't find current state!");
        }
    }

    private void CheckForAttackDistance()
    {
        var distanceForEnemy =  Vector3.Distance(_currentTarget.transform.position , transform.position);

        if (distanceForEnemy <= _attackDistance)
        {
            _currentState = WarriorState.Attack;
            _arguingTrigger.gameObject.SetActive(false); // Disable, becasue it won't need this , before he kills current target
        }
    }

    private void FollowGlobalTarget()
    {
        var direction = (_globalTarget.transform.position - transform.position).normalized;

        _rigidbody2D.velocity = direction * _speedMult;
    }

    private void SetCooldown() => StartCoroutine(CoolDownCoroutine());

    private IEnumerator CoolDownCoroutine()
    {
        _isOnCoolDown = true;

        yield return _waitForSecondsAttackCooldown;

        _isOnCoolDown = false;
    }

    private void GetNewLocalTarget()
    {
        _arguingTrigger.gameObject.SetActive(true);

        
        CurrentTarget = _arguingTrigger.GetNearliestTarget();
    }


    protected virtual void Awake()
    {
        _waitForSecondsAttackCooldown = new WaitForSeconds(_attackCoolDown);

        _arguingTrigger.OnTargetEnter += (HitableEntity target) =>
        {
            if(_isCanArgueOnAreaTarget)
            {
                _currentTarget = target; // If warrioir hasn't target before - chose it and start arguing
                _currentState = WarriorState.Arguing;
                target.OnDie += GetNewLocalTarget;
            }
        };

        _arguingTrigger.OnTargetExit += (HitableEntity target) =>
        {
            // IF it was warrioir target
            if(_currentTarget == target)
            {
                _currentTarget = null;
                _currentState = WarriorState.FollowGlobalTarget;
                target.OnDie -= GetNewLocalTarget;
            }
        };
    }

    protected virtual void Update()
    {
        OnStateUpdate();
    }



    protected enum WarriorState
    {
        Attack,
        Cooldown,
        Arguing,
        FollowGlobalTarget,
    }
}
