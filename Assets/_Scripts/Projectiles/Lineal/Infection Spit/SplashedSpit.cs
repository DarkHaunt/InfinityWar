using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class SplashedSpit : Projectile
    {
        [Range(0.5f, 10f)]
        [SerializeField] private float _splashRadius = 1f;


        protected override void OnCollitionWith(FractionEntity target)
        {
            var collidersInAttackRadius = Physics2D.OverlapCircleAll(target.transform.position, _splashRadius);

            foreach (var collider2D in collidersInAttackRadius)
                if (IsColliderEnemyEntity(collider2D, out FractionEntity enemy))
                    enemy.GetDamage(_damage);

            EndExpluatation();
        }

        protected override void Awake()
        {
            _disptacher = new LinealDispatcher(_speedMult);

            base.Awake();
        }
    } 
}
