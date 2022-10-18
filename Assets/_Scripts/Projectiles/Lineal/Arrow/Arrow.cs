using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class Arrow : LinealProjectile, IRotatable
    {
        protected override void OnCollitionWith(FractionEntity target) => target.GetDamage(_damage);

        public override void ThrowToTarget(Transform targetTransform)
        {
            RoteteToTarget(targetTransform);

            base.ThrowToTarget(targetTransform);
        }

        public void RoteteToTarget(Transform target)
        {
            var xDistance = target.position.x - transform.position.x;
            var yDistance = target.position.y - transform.position.y;

            _rigidbody2D.rotation = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg - 90f;
        }


        protected override void Awake()
        {
            base.Awake();

            OnAffectEnd += () => Destroy(gameObject);
        }
    } 
}


