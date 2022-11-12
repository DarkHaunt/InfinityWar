using InfinityGame.Projectiles;
using InfinityGame.GameEntities;
using UnityEngine;



/// <summary>
/// Action, which will execute when projectile collides with entity
/// </summary>
public abstract class ProjectileEntityCollisionAction : ScriptableObject
{
    public abstract void OnCollisionBehave(Projectile projectile, GameEntity collitionEntity);
}
