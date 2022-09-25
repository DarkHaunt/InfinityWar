using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace InfinityGame.SpawnStrategies
{
    public class GroupSpawnStrategy : ISpawnStrategy
    {
        private float _nextSpawnTime;
        private float _deltaSpawnSeconds;


        public GroupSpawnStrategy(ISpawnStrategy.SpawnCycleData spawnData)
        {
            _nextSpawnTime = spawnData.NextSpawnTime;
            _deltaSpawnSeconds = spawnData.SpawnTimeDelta;
        }


        public IEnumerator SpawnCoroutine(IList<Warrior> warriors)
        {
            throw new System.NotImplementedException();
        }
    }
}
