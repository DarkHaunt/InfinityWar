using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
