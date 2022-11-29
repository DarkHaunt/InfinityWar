using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.GameEntities.Humans
{
    public class Swordman : MeleeWarrior
    {
        [Header("--- Swordman Parameters ---")]

        [Range(0f, 1f)]
        [SerializeField] private float _nonMainTargetDamagePercent;

        [Range(0f, 3f)]
        [SerializeField] private float _swordAtackRadius;

        private float _damageForSurroundedNonMainEntities;



        protected override void Attack()
        {
            var detectedEnemies =  EntitiesDetector.GetEntitiesInArea(transform.position, _swordAtackRadius, Fraction);

            DamageAllSurroundEnemies(detectedEnemies);
        }

        private bool IsMainTarget(GameEntity entity) => LocalTarget == entity;

        private void DamageAllSurroundEnemies(IEnumerable<GameEntity> enemies)
        {
            // Non-main targets around get another damage value
            foreach (var enemy in enemies)
                if (IsMainTarget(enemy))
                    enemy.GetDamage(MeleeDamage);
                else
                    enemy.GetDamage(_damageForSurroundedNonMainEntities);
        }



        protected override void Awake()
        {
            base.Awake();

            _damageForSurroundedNonMainEntities = MeleeDamage * _nonMainTargetDamagePercent;
        }
    }
}
