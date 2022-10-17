using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootStrategy : ScriptableObject
{
    public abstract void Shoot(Transform source, Transform target, Projectile projectilePrefab);
}
