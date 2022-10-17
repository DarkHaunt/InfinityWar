using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;



public abstract class WarrioirsPickStrategy : ScriptableObject
{
    public abstract IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs);
}
