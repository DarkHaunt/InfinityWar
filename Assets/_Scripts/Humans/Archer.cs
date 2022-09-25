using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Warrior, IShoot
{
    [SerializeField] private Bullet _bullerPrefab;


    public void Shoot(Vector3 targetPosition)
    {
        var direction = (targetPosition - transform.position).normalized;
        var bullet = Instantiate(_bullerPrefab);

        bullet.Throw(direction);
    }

    protected override void Attack(IHitable target)
    {
        throw new System.NotImplementedException();
    }

    protected override bool IsOnArguingDistance()
    {
        throw new System.NotImplementedException();
    }
}
