using InfinityGame.Strategies.ProjectileCollisionBehaviors;
using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    public class MultiTouchProjectile : Projectile
    {
        [Header("--- Multi Touch Projectile Settings ---")]

        [SerializeField] private int _maxCollisionsCount = 1;
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
