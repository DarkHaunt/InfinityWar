using System.Collections;
using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    /// <summary>
    /// Ricochets to nearliest enemy in radius
    /// </summary>
    public class Ricochet : MultiTouchProjectile
    {
        [Header("--- Ricochet Parameters ---")]

        [Range(0f, 20f)]
        [Tooltip("Radius of checking new target. If no targets around - destroy bullet")]
        [SerializeField] private float _ricochetRadius = 1f;

        private readonly List<GameEntity> _hittedEnemies = new List<GameEntity>(); // Enemies, which were already hitted in current lyfe cycle of projectile



        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            if (!IsExploitating)
                return;

            _hittedEnemies.Add(target);
            TryToBounce();
        }

        /// <summary>
        /// Tries to dispatch projectile to nearlies existing enemy
        /// </summary>
        private void TryToBounce()
        {
            if (TryToGetClosestEnemyEntity(out GameEntity newTarget))
            {
                var directionToNewTarget = (newTarget.transform.position - transform.position).normalized;
                SetFlyDirection(directionToNewTarget);
            }
            else
                EndExploitation();
        }

        private bool TryToGetClosestEnemyEntity(out GameEntity closestEnemy)
        {
            var enemies = GameEntitiesDetector.GetEntitiesInArea(transform.position, _ricochetRadius, FractionTag);

            var minimalDistance = float.MaxValue;
            closestEnemy = null;

            foreach (var enemy in enemies)
            {
                if (_hittedEnemies.Contains(enemy))
                    continue;

                var distanceToEnemy = Vector2.Distance(enemy.transform.position, transform.position);

                if (distanceToEnemy < minimalDistance)
                {
                    closestEnemy = enemy;
                    minimalDistance = distanceToEnemy;
                }
            }

            // If not found target
            return closestEnemy != null;
        }



        protected override void Awake()
        {
            base.Awake();

            OnExploitationEnd += _hittedEnemies.Clear;
        }
    }
}
