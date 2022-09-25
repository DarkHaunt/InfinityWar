using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.SpawnStrategies
{
    public interface ISpawnStrategy
    {
        IEnumerator SpawnCoroutine(IList<Warrior> warriors);


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

