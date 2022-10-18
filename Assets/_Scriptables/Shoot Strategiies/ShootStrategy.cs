using InfinityGame.Projectiles;
using UnityEngine;


namespace InfinityGame.Strategies.ShootStrategies
{
    public abstract class ShootStrategy : ScriptableObject
    {
        public abstract void Shoot(Transform source, Transform target, Projectile projectilePrefab);
    } 
}
