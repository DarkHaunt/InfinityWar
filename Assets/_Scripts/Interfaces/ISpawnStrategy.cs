using System.Collections;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.SpawnStrategies
{
    public interface ISpawnStrategy
    {
        //IEnumerator SpawnCoroutine(IList<Warrior> warriors);

        /// <summary>
        /// Returns group of warriors depends on strategy realization
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<List<Warrior>> GetWaveWarriors(CancellationToken cancellationToken); // SHOULD BE ASYNC

        //public Transform MainTarget { set; }


        public enum SpawnType
        {
            Group,
            Single
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

