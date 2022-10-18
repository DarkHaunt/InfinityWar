using InfinityGame.Strategies.ShootStrategies;
using InfinityGame.Projectiles;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    public class Shooter : Warrior
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ShootStrategy _shootStrategy;

        protected override void Attack() => _shootStrategy.Shoot(transform, _localTarget.transform, _projectilePrefab);
    } 
}

