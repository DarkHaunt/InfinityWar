using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Threading.Tasks;
using System.Threading;

namespace InfinityGame.SpawnStrategies
{
    public class GroupSpawnStrategy : ISpawnStrategy
    {
        private float _nextSpawnTime;
        private float _deltaSpawnSeconds;

        //private Transform _mainTarget;

        //public Transform MainTarget { set => _mainTarget = value; }

        
        public GroupSpawnStrategy(ISpawnStrategy.SpawnCycleData spawnData)
        {
            _nextSpawnTime = spawnData.NextSpawnTime;
            _deltaSpawnSeconds = spawnData.SpawnTimeDelta;
        }

/*        public IEnumerator SpawnCoroutine(IList<Warrior> warriors)
        {


            throw new System.NotImplementedException();
        }*/

        public Task<List<Warrior>> GetWaveWarriors(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
