using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


namespace InfinityGame.Strategies.WarrioirPickStrategies
{
    [CreateAssetMenu(fileName = "SpawnStrategy", menuName = "Data/WarrioirChoseStrategies/RandomGroupPick", order = 52)]
    public class RandomGroupPick : WarrioirsPickStrategy
    {
        [SerializeField] private int _groupMembersCount;


        public override IEnumerable<Warrior> ChoseWarrioirsToSawn(IReadOnlyList<Warrior> warrioirs)
        {
            for (int i = 0; i < _groupMembersCount; i++)
                yield return warrioirs[StaticRandomizer.Randomizer.Next(0, warrioirs.Count)];
        }
    } 
}
