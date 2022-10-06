using System.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.SpawnStrategies
{
    public interface IWarrioirChoseStrategy
    {
        /// <summary>
        /// Returns group of warriors depends on strategy realization
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        IEnumerable<Warrior> ChoseWarrioirsToSawn(IList<Warrior> warrioirs); // SHOULD BE ASYNC

        //public Transform MainTarget { set; }


        public enum SpawnType
        {
            RandomGroup, // Group of random warrioirs
            RandomSingle, // Random single warrioir
            QueueSingle, // Picks single warrioir in course from all avaliable warrioirs
            QueueGroup // Picks concrete warrioir group
        };


        [Serializable]
        public struct SpawnCycleData
        {
            public float NextSpawnTime;
            public float SpawnTimeDelta;
            public SpawnType SpawnType;


            public SpawnCycleData(float nextSpawnTime, float spawnTimeDelta, SpawnType spawnType)
            {
                NextSpawnTime = nextSpawnTime;
                SpawnTimeDelta = spawnTimeDelta;
                SpawnType = spawnType;
            }
        }
    }
}

