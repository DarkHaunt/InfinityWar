using InfinityGame.GameEntities;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    /// <summary>
    /// Behavior of projectile on collision with GameEntity target
    /// </summary>
    public abstract class ProjectileColliisionBehavior : ScriptableObject
    {
        public abstract void OnCollisionBehave(GameEntity target, Projectile projectile);
    }
}
