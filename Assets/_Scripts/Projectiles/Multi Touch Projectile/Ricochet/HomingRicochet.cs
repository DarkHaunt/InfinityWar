using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Projectiles
{
    /// <summary>
    /// After hit of GameEntity tries to find a new nearliest GameEntity target in radius
    /// If have found - bounce to new target
    /// Don't bounce to target, if it was already hitted by this projectile
    /// </summary>
    public class HomingRicochet : MultiTouchProjectile
    {
        [Header("--- Ricochet Parameters ---")]

        [Range(0f, 20f)]
        [Tooltip("Radius of checking new target. If no targets around - destroy bullet")]
        [SerializeField] private float _ricochetRadius = 1f;

        private readonly HashSet<GameEntity> _hittedEnemies = new HashSet<GameEntity>(); // Enemies, which were already hitted in current lyfe cycle of projectile



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
            var entitiesAround = EntitiesDetector.GetEntitiesInArea(transform.position, _ricochetRadius, FractionTag);
            var nonHittedEntitiesAround = GetNonHittedEntitiesFrom(entitiesAround);

            closestEnemy = EntitiesDetector.TryToGetClosestEntityToPosition(transform.position, nonHittedEntitiesAround);

            // If not found target
            return closestEnemy != null;
        }

        /// <summary>
        /// Makes sure, that projectile won't hit the same target in it's lyfe cycle
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private IEnumerable<GameEntity> GetNonHittedEntitiesFrom(IEnumerable<GameEntity> entities)
        {
            foreach (var entity in entities)
                if (!_hittedEnemies.Contains(entity))
                    yield return entity;
        }



        protected override void Awake()
        {
            base.Awake();

            OnExploitationEnd += _hittedEnemies.Clear;
        }
    }
}
