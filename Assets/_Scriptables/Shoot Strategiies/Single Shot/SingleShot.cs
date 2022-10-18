using InfinityGame.Factories.ProjectileFactory;
using InfinityGame.Projectiles;
using UnityEngine;


namespace InfinityGame.Strategies.ShootStrategies
{
    [CreateAssetMenu(fileName = "ShootStrategy", menuName = "Data/ShootStrategy/SingleShot", order = 52)]
    public class SingleShot : ShootStrategy
    {
        public override void Shoot(Transform source, Transform target, Projectile projectilePrefab)
        {
            var bullet = ProjectileFactory.Instantiate(projectilePrefab);
            bullet.transform.position = source.position;
            bullet.ThrowToTarget(target);
        }
    }
}