using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    public class MultiTouchProjectile : Projectile
    {
        [Header("--- Multi Touch Projectile Settings ---")]
        [SerializeField] private int _maxCollisionsCount = 1;
        [SerializeField] private List<ProjectileEntityCollisionAction> _lastCollisionActions;

        private int _collisionsCount = 0;



        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            _collisionsCount++;

            if (IsCollitionsOverflow())
            {
                OnLastCollition(target);
                EndExploitation();
                return;
            }

            RestartLifeTime();
        }

        private void OnLastCollition(GameEntity target)
        {
            foreach (var projectileBehavior in _lastCollisionActions)
                projectileBehavior.OnCollisionBehave(this, target);
        }

        private bool IsCollitionsOverflow() => _collisionsCount >= _maxCollisionsCount;


        protected override void Awake()
        {
            base.Awake();

            OnExploitationEnd += () => _collisionsCount = 0;
        }
    }
}
