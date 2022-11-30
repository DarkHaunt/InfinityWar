using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Strategies.WarrioirPickStrategies
{
    /// <summary>
    /// Determines, which warriors of collection should be picked and which order
    /// </summary>
    public abstract class WarrioirsPickStrategy : ScriptableObject
    {
        public abstract IEnumerable<Warrior> ChoseWarrioirsToSpawn(IReadOnlyList<Warrior> warrioirs);
    } 
}
