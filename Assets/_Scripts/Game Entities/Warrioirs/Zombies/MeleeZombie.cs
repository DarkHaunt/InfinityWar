


namespace InfinityGame.GameEntities.Zombies
{
    public class MeleeZombie : MeleeWarrior
    {
        protected override void Attack()
        {
            LocalTarget.GetDamage(MeleeDamage);
        }
    } 
}
