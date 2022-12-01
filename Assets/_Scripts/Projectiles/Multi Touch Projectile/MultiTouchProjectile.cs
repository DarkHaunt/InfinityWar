using InfinityGame.Strategies.ProjectileCollisionBehaviors;
using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    /// <summary>
    /// Projectile, which can collide with GameEntity many times
    /// </summary>
    public class MultiTouchProjectile : Projectile
    {
        [Header("--- Multi Touch Projectile Settings ---")]

        [Min(2)] // Because it's the minimal value, which makes sence to use this class 
        [SerializeField] private int _maxCollisionsCount = 2;
        [SerializeField] private List<ProjectileColliisionBehavior> _lastCollisionBehaviors;

        private LimitCounter _collisionCounter;



        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            _collisionCounter.Increase();

            if (_collisionCounter.IsOverflowed)
                OnLastCollision(target);
            else
                RestartLifeTime();
        }

        private void OnLastCollision(GameEntity target)
        {
            foreach (var projectileBehavior in _lastCollisionBehaviors)
                projectileBehavior.OnCollisionBehave(target, this);
        }




        protected override void Awake()
        {
            base.Awake();

            _collisionCounter = new LimitCounter(_maxCollisionsCount);
            _collisionCounter.OnCounterLimitOverflow += EndExploitation;

            OnExploitationEnd += _collisionCounter.ResetCounter;
        }
    }
}
