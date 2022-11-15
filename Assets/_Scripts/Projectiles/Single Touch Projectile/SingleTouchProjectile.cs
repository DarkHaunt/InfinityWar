using InfinityGame.GameEntities;



namespace InfinityGame.Projectiles
{
    public class SingleTouchProjectile : Projectile
    {
        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            EndExploitation();
        }
    } 
}
