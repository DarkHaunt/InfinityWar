using System.Collections;
using System.Collections.Generic;
using InfinityGame.WarriorFactory;
using UnityEngine;


namespace InfinityGame.SpawnStrategies
{
    public class SingleSpawnStrategy : ISpawnStrategy
    {
        private float _nextSpawnTime;
        private float _deltaSpawnSeconds;


        public SingleSpawnStrategy(ISpawnStrategy.SpawnCycleData spawnData)
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