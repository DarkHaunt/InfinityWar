using InfinityGame.Projectiles;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Strategies.ProjectileCollisionAction
{
    /// <summary>
    /// Action, which will execute when projectile collides with entity
    /// </summary>
    public abstract class ProjectileEntityCollisionAction : ScriptableObject
    {
        public abstract void OnCollisionBehave(Projectile projectile, GameEntity collitionEntity);
    } 
}
