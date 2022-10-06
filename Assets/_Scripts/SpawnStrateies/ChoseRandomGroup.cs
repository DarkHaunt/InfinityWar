using System.Collections;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace InfinityGame.SpawnStrategies
{
    public class ChoseRandomGroup : IWarrioirChoseStrategy
    {
        private Random _randomizer = new Random();
        private int _groupMembersCount;


        public ChoseRandomGroup(int groupMembersCount) => _groupMembersCount = groupMembersCount;
        

        public IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs)
        {
            for (int i = 0; i < _groupMembersCount; i++)
                yield return warrioirs[_randomizer.Next(0, warrioirs.Count)];
        }
    }
}
