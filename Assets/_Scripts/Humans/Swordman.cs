using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swordman : Warrior, IMelee
{
    [SerializeField] private float _damage;
    [SerializeField] private float _coolDown;

    [Range(0f, 1f)]
    [SerializeField] private float _nonMainTargetDamage;


    public bool IsOppositeSideWarrior()
    {
        throw new System.NotImplementedException();
    }

    protected override void Attack(IHitable target)
    {
        


    }

    protected override bool IsOnArguingDistance()
    {
        throw new System.NotImplementedException();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
