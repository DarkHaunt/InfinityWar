using InfinityGame.Projectiles;
using UnityEngine;



namespace InfinityGame.Strategies.ShootStrategies
{
    /// <summary>
    /// The way of how shooter should to shoot
    /// </summary>
    public abstract class ShootStrategy : ScriptableObject
    {
        public abstract void Shoot(ShooterData shooterData, Projectile projectilePrefab);



        public struct ShooterData
        {
            public Vector2 FirePosition { get; private set; }
            public Vector2 ShootDirection { get; private set; }
            public string ShooterFraction { get; private set; }



            public ShooterData(Vector2 firePosition, Vector2 shootDirection, string shooterFraction)
            {
                FirePosition = firePosition;
                ShootDirection = shootDirection;
                ShooterFraction = shooterFraction;
            }
        }
    }
}
