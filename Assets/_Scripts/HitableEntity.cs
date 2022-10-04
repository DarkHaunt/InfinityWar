using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HitableEntity : MonoBehaviour, IFractionTagable
{
    public event Action OnDie;
    public event Action OnHit;

    [SerializeField] protected float _health;
    protected string _fractionTag;


    public string FractionTag => _fractionTag;



    public void GetDamage(float damage)
    {
        _health -= damage;

        if (_health <= 0)
        {
            OnDie?.Invoke();
            Destroy(this);
            return;
        }

        OnHit?.Invoke();
    }
}
