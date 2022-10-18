using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class InfectedSpit : LinealProjectile
    {
        [Range(0.5f, 10f)]
        [SerializeField] private float _explosionRadius = 1f;


        protected override void OnCollitionWith(FractionEntity target)
        {
            var collidersInAttackRadius = Physics2D.OverlapCircleAll(target.transform.position, _explosionRadius);

            foreach (var collider2D in collidersInAttackRadius)
                if (ColliderIsEnemyEntity(collider2D, out FractionEntity enemy))
                    enemy.GetDamage(_damage);
        }


        protected override void Awake()
        {
            base.Awake();

            OnAffectEnd += () => Destroy(gameObject);
        }
    } 
}
