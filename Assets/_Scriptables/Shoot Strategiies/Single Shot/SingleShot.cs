using InfinityGame.Factories.ProjectileFactory;
using InfinityGame.Projectiles;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Strategies.ShootStrategies
{
    [CreateAssetMenu(fileName = "ShootStrategy", menuName = "Data/ShootStrategy/SingleShot", order = 52)]
    public class SingleShot : ShootStrategy
    {
        public override void Shoot(Shooter shooter, GameEntity target, Projectile projectilePrefab)
        {
            var projectile = ProjectileFactory.Instantiate(projectilePrefab, shooter.Fraction);
            projectile.transform.position = shooter.transform.position;
            projectile.HeadTowardsTarget(target.transform);
        }
    }
}