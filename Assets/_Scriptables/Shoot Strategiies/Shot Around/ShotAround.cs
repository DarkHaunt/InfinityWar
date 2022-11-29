using InfinityGame.Factories.ProjectileFactory;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ShootStrategies
{
    [CreateAssetMenu(fileName = "ShootStrategy", menuName = "Data/ShootStrategy/ShotAround", order = 52)]
    public class ShotAround : ShootStrategy
    {
        [Range(1, 360)]
        [SerializeField] private int _totalBulletsCount = 1;



        public override void Shoot(ShooterData shooterData, Projectile projectilePrefab)
        {
            var angleBetweenBullets = (360 / _totalBulletsCount) * Mathf.Deg2Rad;
            var currentAngleInRadians = TrigonometryCalculator.GetRotationAngleToPoint(shooterData.ShootDirection);


            for (int i = 0; i < _totalBulletsCount; i++, currentAngleInRadians += angleBetweenBullets)
            {
                var projectile = ProjectileFactory.Instantiate(projectilePrefab, shooterData.FirePosition, shooterData.ShooterFraction);

                var currentDirectionX = Mathf.Cos(currentAngleInRadians);
                var currentDirectionY = Mathf.Sin(currentAngleInRadians);

                var currentDirection = new Vector2(currentDirectionX, currentDirectionY);

                projectile.SetFlyDirection(currentDirection.normalized);
            }
        }
    }
}
