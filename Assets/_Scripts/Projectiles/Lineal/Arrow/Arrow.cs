using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class Arrow : RotatableProjectile
    {
        protected override void OnCollisionWith(GameEntity target)
        {
            target.GetDamage(Damage);

            EndExploitation();
        }



        protected override void Awake()
        {
            InitializeDispatcher(new LinealDispatcher(Speed));
            InitializeRotationStrategy(new RotateToTargetOnce());

            base.Awake();
        }
    } 
}


