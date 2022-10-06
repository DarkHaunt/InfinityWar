using System.Collections;
using System.Collections.Generic;
using InfinityGame.WarriorFactory;
using System;
using System.Threading.Tasks;
using System.Threading;


namespace InfinityGame.SpawnStrategies
{
    public class ChoseRandomSingle : IWarrioirChoseStrategy
    {
        private Random _randomizer = new Random();

        public ChoseRandomSingle() { }


        public IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs)
        {
            var chosenIndex = _randomizer.Next(0, warrioirs.Count);

            yield return warrioirs[chosenIndex];
        }
    }
}