using InfinityGame.Strategies.ShootStrategies;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    using ShooterData = ShootStrategy.ShooterData;


    public class Shooter : Warrior
    {
        [Header("--- Shooting Parameters ---")]
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ShootStrategy _shootStrategy;



        protected override void Attack()
        {
            var shootDirection = GetDirectionToLocalTarget();
            var shooterData = new ShooterData(transform.position, shootDirection, Fraction);
            _shootStrategy.Shoot(shooterData, _projectilePrefab);
        } 

        private Vector2 GetDirectionToLocalTarget() => (LocalTarget.transform.position - transform.position).normalized;
    }
}

