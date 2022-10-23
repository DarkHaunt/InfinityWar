using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class Arrow : RotatableProjectile
    {
        protected override void OnCollitionWith(FractionEntity target)
        {
            target.GetDamage(Damage);

            EndExpluatation();
        }

        protected override void Awake()
        {
            _disptacher = new LinealDispatcher(Speed);
            _rotateStrategy = new RotateToTargetOnce();

            base.Awake();
        }
    } 
}


