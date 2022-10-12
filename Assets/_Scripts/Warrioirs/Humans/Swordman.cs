using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.Fractions.Humans
{
    public class Swordman : Warrior
    {
        [Range(0f, 1f)]
        [SerializeField] private float _nonMainTargetDamagePercent;

        [Range(0f, 3f)]
        [SerializeField] private float _atackRadius;

        private float _damageForSurroundedNonMainEntities;


        protected override void Attack()
        {
            var collidersInAttackRadius = Physics2D.OverlapCircleAll(transform.position, _atackRadius);

            var enemies = GetEnemiesFrom(collidersInAttackRadius);
            DamageAllSurroundEnemies(enemies);
        }

        private bool IsColliderHostileEntity(Collider2D collider2D, out FractionEntity hostileEntity)
        {
            var isHitableEntity = collider2D.TryGetComponent(out FractionEntity entity);

            hostileEntity = entity;
            return isHitableEntity && !entity.IsSameFraction(FractionTag);
        }

        private bool IsMainTarget(FractionEntity entity) => _localTarget == entity;

        private void DamageAllSurroundEnemies(IEnumerable<FractionEntity> enemies)
        {
            // Non-main targets around get another damage value
            foreach (var enemy in enemies)
                if (IsMainTarget(enemy))
                    enemy.GetDamage(_damage);
                else
                    enemy.GetDamage(_damageForSurroundedNonMainEntities);
        }

        private IEnumerable<FractionEntity> GetEnemiesFrom(Collider2D[] colliders)
        {
            foreach (var collider in colliders)
                if (IsColliderHostileEntity(collider, out FractionEntity hostileEntity))
                    yield return hostileEntity;
        }


        protected override void Awake()
        {
            base.Awake();

            _damageForSurroundedNonMainEntities = _damage * _nonMainTargetDamagePercent;
        }
    }
}
