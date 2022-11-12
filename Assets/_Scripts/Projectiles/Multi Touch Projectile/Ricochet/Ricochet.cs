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
        [SerializeField] private float _ricochetRadius = 1f;

        private FractionEntityDetector _entityDetector;

        [SerializeField] private List<GameEntity> _hittedEnemies = new List<GameEntity>();



        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            if (!IsExploitating)
                return;

            _hittedEnemies.Add(target);

            if (TryToGetClosestEnemyEntity(out GameEntity newTarget))
            {
                var newFlyDirection = (newTarget.transform.position - transform.position).normalized;
                SetFlyDirection(newFlyDirection);
            }
            else
                EndExploitation();
        }

        private bool TryToGetClosestEnemyEntity(out GameEntity closestEnemy)
        {
            var enemies = _entityDetector.GetDetectedFractionEntities(transform.position);
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

            OnTagChange += (string newTag) =>
            {
                _entityDetector = new FractionEntityDetector(_ricochetRadius, newTag);
            };

            OnExploitationEnd += _hittedEnemies.Clear;
        }
    }
}
