using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    public class MultiTouchProjectile : Projectile
    {
        [SerializeField] private int _maxCollitionsCount = 1;
        [SerializeField] private List<ProjectileEntityCollisionAction> _lastCollitionBehaviors;

        private int _collitionCount = 0;



        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            if (IsCollitionsOverflow())
            {
                OnLastCollition(target);
                EndExploitation();
            }
            else
                _collitionCount++;
        }

        private void OnLastCollition(GameEntity target)
        {
            foreach (var projectileBehavior in _lastCollitionBehaviors)
                projectileBehavior.OnCollisionBehave(this, target);
        }

        private bool IsCollitionsOverflow() => _collitionCount >= _maxCollitionsCount;

    }
}
