using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.GameEntities.Zombies
{
    public class MeleeZombie : MeleeWarrioir
    {
        protected override void Attack()
        {
            _localTarget.GetDamage(_meleeDamage);
        }
    } 
}
