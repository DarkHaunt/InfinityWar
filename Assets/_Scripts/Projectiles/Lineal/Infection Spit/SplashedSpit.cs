using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class SplashedSpit : Projectile
    {
        [Range(0.5f, 10f)]
        [SerializeField] private float _splashRadius = 1f;



        protected override void OnCollisionWith(FractionEntity target)
        {
            var collidersInAttackRadius = Physics2D.OverlapCircleAll(target.transform.position, _splashRadius);

            foreach (var collider2D in collidersInAttackRadius)
                if (IsColliderHasEnemyFractionEntityComponent(collider2D, out FractionEntity enemy))
                    enemy.GetDamage(Damage);

            EndExpluatation();
        }



        protected override void Awake()
        {
            InitializeDispatcher(new LinealDispatcher(Speed));

            base.Awake();
        }
    } 
}
