using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : Warrior
{
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private ShootStrategy _shootStrategy;

    protected override void Attack() => _shootStrategy.Shoot(transform ,_localTarget.transform, _projectilePrefab);
}

