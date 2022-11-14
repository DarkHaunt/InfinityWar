using InfinityGame.GameEntities;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    /// <summary>
    /// Simple hit behavior
    /// </summary>
    [CreateAssetMenu(fileName = "SimpleHitBehavior", menuName = "Data/Projectile Collision Behaviors/SimpleHit", order = 52)]
    public class SimpleHit : DamageDealBehavior
    {
        public override void OnCollisionBehave(GameEntity target, Projectile projectile)
        {
            target.GetDamage(Damage);
        }
    } 
}
