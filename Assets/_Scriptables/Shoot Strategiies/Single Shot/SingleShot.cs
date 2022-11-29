using InfinityGame.Factories.ProjectileFactory;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ShootStrategies
{
    [CreateAssetMenu(fileName = "ShootStrategy", menuName = "Data/ShootStrategy/SingleShot", order = 52)]
    public class SingleShot : ShootStrategy
    {
        public override void Shoot(ShooterData shooterData,Projectile projectilePrefab)
        {
            var projectile = ProjectileFactory.Instantiate(projectilePrefab, shooterData.FirePosition, shooterData.ShooterFraction);
            projectile.SetFlyDirection(shooterData.ShootDirection);
        }
    }
}