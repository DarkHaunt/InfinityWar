using InfinityGame.GameEntities;



namespace InfinityGame.Projectiles
{
    /// <summary>
    /// Projectile, which ends it expluatation after first collision with GameEntity
    /// </summary>
    public class SingleTouchProjectile : Projectile
    {
        protected override void OnCollisionWith(GameEntity target)
        {
            base.OnCollisionWith(target);

            EndExploitation();
        }
    } 
}
