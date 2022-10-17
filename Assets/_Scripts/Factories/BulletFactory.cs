using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BulletFactory
{
    public static ProjectileType Instantiate<ProjectileType>(ProjectileType prefab) where ProjectileType : Projectile => MonoBehaviour.Instantiate(prefab);
}
