using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;



namespace InfinityGame.Strategies.WarrioirPickStrategies
{
    /// <summary>
    /// Pick group of random warrioirs from collection
    /// </summary>
    [CreateAssetMenu(fileName = "RandomGroupPickStrategy", menuName = "Data/WarrioirChoseStrategies/RandomGroupPick", order = 52)]
    public class RandomGroupPickStrategy : WarrioirsPickStrategy
    {
        [SerializeField] private int _groupMembersCount;



        public override IEnumerable<Warrior> ChoseWarrioirsToSpawn(IReadOnlyList<Warrior> warrioirs)
        {
            for (int i = 0; i < _groupMembersCount; i++)
                yield return warrioirs[StaticRandomizer.Randomizer.Next(0, warrioirs.Count)];
        }
    } 
}
