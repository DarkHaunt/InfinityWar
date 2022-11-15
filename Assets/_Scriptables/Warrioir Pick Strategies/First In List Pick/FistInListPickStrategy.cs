using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Strategies.WarrioirPickStrategies
{
    [CreateAssetMenu(fileName = "FistInListPickStrategy", menuName = "Data/WarrioirChoseStrategies/FistInListPickStrategy", order = 52)]
    public class FistInListPickStrategy : WarrioirsPickStrategy
    {
        public override IEnumerable<Warrior> ChoseWarrioirsToSpawn(IReadOnlyList<Warrior> warrioirs)
        {
            yield return warrioirs[0];
        }
    }
}