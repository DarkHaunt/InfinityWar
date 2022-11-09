using InfinityGame.Factories.ProjectileFactory;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ShootStrategies
{
    [CreateAssetMenu(fileName = "ShootStrategy", menuName = "Data/ShootStrategy/SingleShot", order = 52)]
    public class SingleShot : ShootStrategy
    {
        public override void Shoot(Vector2 position, Vector2 direction, string fraction,Projectile projectilePrefab)
        {
            var projectile = ProjectileFactory.Instantiate(projectilePrefab, position ,fraction);
            projectile.SetFlyDirection(direction);
        }
    }
}