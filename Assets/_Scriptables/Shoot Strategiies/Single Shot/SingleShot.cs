using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "ShootStrategy", menuName = "Data/ShootStrategy/SingleShot", order = 52)]
public class SingleShot : ShootStrategy
{
    public override void Shoot(Transform source, Transform target, Projectile projectilePrefab)
    {
        var bullet = BulletFactory.Instantiate(projectilePrefab);
        bullet.transform.position = source.position;
        bullet.Throw(target);
    }
}
