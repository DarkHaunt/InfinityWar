using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


namespace InfinityGame.Strategies.WarrioirPickStrategies
{
    public abstract class WarrioirsPickStrategy : ScriptableObject
    {
        public abstract IEnumerable<Warrior> ChoseWarrioirsToSawn(IReadOnlyList<Warrior> warrioirs);
    } 
}
