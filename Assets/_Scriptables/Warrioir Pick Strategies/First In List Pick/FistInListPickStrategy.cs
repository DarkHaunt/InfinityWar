using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.Strategies.WarrioirPickStrategies
{
    [CreateAssetMenu(fileName = "FistInListPickStrategy", menuName = "Data/WarrioirChoseStrategies/FistInListPickStrategy", order = 52)]
    public class FistInListPickStrategy : WarrioirsPickStrategy
    {
        [SerializeField] private int _countOfPicks = 1;

        public override IEnumerable<Warrior> ChoseWarrioirsToSpawn(IReadOnlyList<Warrior> warrioirs)
        {
            for (int i = 0; i < _countOfPicks; i++)
                yield return warrioirs[0];
        }
    }
}