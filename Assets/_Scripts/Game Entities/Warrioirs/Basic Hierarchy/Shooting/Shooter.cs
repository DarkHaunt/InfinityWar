using InfinityGame.Strategies.ShootStrategies;
using InfinityGame.Projectiles;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    public class Shooter : Warrior
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private ShootStrategy _shootStrategy;



        protected override void Attack() => _shootStrategy.Shoot(transform.position, GetDirectionToLocalTarget(), Fraction, _projectilePrefab);

        private Vector2 GetDirectionToLocalTarget() => (LocalTarget.transform.position - transform.position).normalized;
    }
}

