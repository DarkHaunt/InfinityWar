using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


namespace InfinityGame.Strategies.WarrioirSpawnStrategies
{
    [CreateAssetMenu(fileName = "SpawnStrategy", menuName = "Data/WarrioirChoseStrategies/RandomSinglePick", order = 52)]
    public class RandomSinglePickStrategy : WarrioirsPickStrategy
    {
        private System.Random _randomizer = new System.Random();

        public override IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs)
        {
            var chosenIndex = _randomizer.Next(0, warrioirs.Count);

            yield return warrioirs[chosenIndex];
        }
    } 
}
