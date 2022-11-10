using InfinityGame.GameEntities;



namespace InfinityGame.Projectiles
{
    public class SimpleProjectile : RotatableProjectile
    {
        protected override void OnCollisionWith(GameEntity target)
        {
            target.GetDamage(Damage);

            EndExploitation();
        }
    } 
}


