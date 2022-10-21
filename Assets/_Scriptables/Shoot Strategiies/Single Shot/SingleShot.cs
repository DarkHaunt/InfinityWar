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
            var projectile = ProjectileFactory.Instantiate(projectilePrefab);
            projectile.transform.position = source.position;
            projectile.HeadTowardsTarget(target);
        }
    }
}