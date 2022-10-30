using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


namespace InfinityGame.Strategies.WarrioirSpawnStrategies
{
    [CreateAssetMenu(fileName = "SpawnStrategy", menuName = "Data/WarrioirChoseStrategies/RandomSinglePick", order = 52)]
    public class RandomSinglePickStrategy : WarrioirsPickStrategy
    {
        public override IEnumerable<Warrior> ChoseWarrioirsToSawn(IReadOnlyList<Warrior> warrioirs)
        {
            var chosenIndex = StaticRandomizer.Randomizer.Next(0, warrioirs.Count);

            yield return warrioirs[chosenIndex];
        }
    } 
}
