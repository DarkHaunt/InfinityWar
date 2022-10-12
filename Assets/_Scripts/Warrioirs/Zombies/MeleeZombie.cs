using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeZombie : Warrior
{
    protected override void Attack()
    {
        _localTarget.GetDamage(_damage);
    }
}
