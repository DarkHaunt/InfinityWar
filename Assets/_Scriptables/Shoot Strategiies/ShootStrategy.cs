using InfinityGame.Projectiles;
using InfinityGame.Fractions;
using InfinityGame.GameEntities;
using UnityEngine;


namespace InfinityGame.Strategies.ShootStrategies
{
    public abstract class ShootStrategy : ScriptableObject
    {
        public abstract void Shoot(Shooter shooter, GameEntity target, Projectile projectilePrefab);
    } 
}
