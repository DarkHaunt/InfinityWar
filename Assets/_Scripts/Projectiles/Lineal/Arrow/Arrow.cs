using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class Arrow : RotatableProjectile
    {
        protected override void OnCollisionWith(FractionEntity target)
        {
            target.GetDamage(Damage);

            EndExpluatation();
        }



        protected override void Awake()
        {
            InitializeDispatcher(new LinealDispatcher(Speed));
            InitializeRotationStrategy(new RotateToTargetOnce());

            base.Awake();
        }
    } 
}


