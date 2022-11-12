using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ShootStrategies
{
    public abstract class ShootStrategy : ScriptableObject
    {
        public abstract void Shoot(Vector2 position, Vector2 direction, string fraction, Projectile projectilePrefab);
    }
}
