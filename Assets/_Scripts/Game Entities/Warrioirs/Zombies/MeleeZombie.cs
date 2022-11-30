


namespace InfinityGame.GameEntities.MeleeWarriors
{
    public class MeleeZombie : MeleeWarrior
    {
        protected override void Attack()
        {
            LocalTarget.GetDamage(MeleeDamage);
        }
    } 
}
