using InfinityGame.Projectiles;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionAction
{
    /// <summary>
    /// Doing splash damage around the radius of collided target
    /// </summary>
    [CreateAssetMenu(fileName = "ProjectileBehavior", menuName = "Data/Projectile Collision Action/SplashDamage", order = 52)]
    public class SplashDamage : ProjectileEntityCollisionAction
    {
        [Range(0f, 10f)]
        [SerializeField] private float _splashRadius = 1f;



        public override void OnCollisionBehave(Projectile projectile, GameEntity target)
        {
            var detectedEnemies = GameEntitiesDetector.GetEntitiesInArea(target.transform.position, _splashRadius, projectile.FractionTag);

            foreach (var enemy in detectedEnemies)
                enemy.GetDamage(projectile.Damage);
        }
    }

}