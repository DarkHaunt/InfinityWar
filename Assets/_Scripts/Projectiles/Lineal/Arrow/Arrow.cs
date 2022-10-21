using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class Arrow : RotatableProjectile
    {
        protected override void OnCollitionWith(FractionEntity target)
        {
            target.GetDamage(_damage);

            EndExpluatation();
        }

        protected override void Awake()
        {
            _disptacher = new LinealDispatcher(_speedMult);
            _rotateStrategy = new RotateToTargetOnce();

            base.Awake();
        }
    } 
}


