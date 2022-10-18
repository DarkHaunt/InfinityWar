using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


namespace InfinityGame.Strategies.WarrioirSpawnStrategies
{
    public abstract class WarrioirsPickStrategy : ScriptableObject
    {
        public abstract IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs);
    } 
}
