using InfinityGame.Projectiles;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    /// <summary>
    /// Doing splash damage around the radius of collided target
    /// </summary>
    [CreateAssetMenu(fileName = "SplashDamageBehavior", menuName = "Data/Projectile Collision Behaviors/SplashDamage", order = 52)]
    public class SplashDamage : DamageDealBehavior
    {
        [Range(0f, 10f)]
        [SerializeField] private float _splashRadius = 1f;



        public override void OnCollisionBehave(GameEntity target, Projectile projectile)
        {
            var detectedEnemies = GameEntitiesDetector.GetEntitiesInArea(target.transform.position, _splashRadius, projectile.FractionTag);

            foreach (var enemy in detectedEnemies)
                enemy.GetDamage(Damage);
        }
    }

}