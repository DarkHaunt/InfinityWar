using InfinityGame.GameEntities;
using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionBehaviors
{
    public abstract class ProjectileColliisionBehavior : ScriptableObject
    {
        public abstract void OnCollisionBehave(GameEntity target, Projectile projectile);
    }
}
