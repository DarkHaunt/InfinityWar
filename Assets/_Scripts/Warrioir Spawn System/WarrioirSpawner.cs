using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using InfinityGame.Strategies.WarrioirPickStrategies;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.GameEntities;
using InfinityGame.DataCaching;
using InfinityGame.FractionsData;



namespace InfinityGame.Spawning
{
    /// <summary>
    /// Loop spawning certain count of warriors determied by spawn strategy
    /// </summary>
    public class WarrioirSpawner : MonoBehaviour
    {
        // A max scatter between warrior spawn position and spawner position in units
        private static Vector2 MaxSpawnPositionScatter = new Vector2(2f, 2f);

        private float _spawnCoolDownSeconds;
        private float _spawnTimeDeviationSeconds; // A scatter of next spawn time (+- this value) 

        private IReadOnlyList<Warrior> _warriorsToSpawn;
        private WarrioirsPickStrategy _warriorsPickStrategy;
        private string _fractionTag;

        private bool _isSpawning = true;

        private Coroutine _spawnCoroutine;



        public string SpawnerFraction => _fractionTag;



        public void Initialize(FractionInitData fractionData) => Initialize(fractionData.FractionTag, fractionData.BarracksWarrioirSpawnSettings);

        public void Initialize(string fractionTag, SpawnerInitData spawnData)
        {
            _spawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
            _spawnTimeDeviationSeconds = spawnData.SpawnTimeDeviationSeconds;

            _fractionTag = fractionTag;

            if (!IsAllWarrioirsBelongsToSpawnerFraction(spawnData.WarriosToSpawn))
                throw new UnityException($"Not all warrioirs belongs to fraction {_fractionTag} in spawner {gameObject.name}");

            if (_spawnTimeDeviationSeconds >= _spawnCoolDownSeconds)
                throw new UnityException($"Spawn delay can't be equal or higher than cool down seconds!");

            _warriorsToSpawn = spawnData.WarriosToSpawn;
            _warriorsPickStrategy = spawnData.WarrioirsPickStrategy;


            ActivateSpawner();
        }

        public void ActivateSpawner()
        {
            FractionCacher.PutSpawnerOnWarrioirRecord(this);
            StartSpawning();
        }

        public void DeactivateSpawner()
        {
            FractionCacher.OutputSpawnerFromWarrioirRecord(this);
            StopSpawning();
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            GameInitializer.OnGameEnd += DeactivateSpawner;
            _spawnCoroutine = StartCoroutine(SpawnCoroutine());
        }

        public void StopSpawning()
        {
            _isSpawning = false;
            GameInitializer.OnGameEnd -= DeactivateSpawner;
            StopCoroutine(_spawnCoroutine);
        }

        private bool IsAllWarrioirsBelongsToSpawnerFraction(IEnumerable<Warrior> warriors)
        {
            foreach (var warrior in warriors)
                if (!warrior.IsBelongsToFraction(SpawnerFraction))
                    return false;

            return true;
        }

        private IEnumerator SpawnCoroutine()
        {
            while (true)
            {
                yield return new WaitForSeconds(NewSpawnDelaySeconds());

                var pickedWarioirsPrefabs = _warriorsPickStrategy.ChoseWarrioirsToSpawn(_warriorsToSpawn);


                foreach (var warrioirPrefab in pickedWarioirsPrefabs)
                {
                    if (!_isSpawning)
                        break;

                    var warrioirPosition = transform.position + RandomPositionDeviation();
                    WarriorFactory.InstantiateWarrior(warrioirPrefab, warrioirPosition);
                }
            }
        }

        /// <summary>
        /// Coordinates in {-MaxSpawnPositionScatter, MaxSpawnPositionScatter} diapason
        /// </summary>
        /// <returns></returns>
        private Vector3 RandomPositionDeviation()
        {
            var randomX = (float)(StaticRandomizer.GetRandomSign() * StaticRandomizer.Randomizer.NextDouble() * MaxSpawnPositionScatter.x);
            var randomY = (float)(StaticRandomizer.GetRandomSign() * StaticRandomizer.Randomizer.NextDouble() * MaxSpawnPositionScatter.y);

            return new Vector3(randomX, randomY);
        }

        private float NewSpawnDelaySeconds()
        {
            var randomPercentOfDeltaMiliseconds = StaticRandomizer.Randomizer.NextDouble(); // A percentage scaling of the spawn time deviation 
            var rawTime = (_spawnCoolDownSeconds + (StaticRandomizer.GetRandomSign() * _spawnTimeDeviationSeconds * randomPercentOfDeltaMiliseconds));

            return (float)rawTime;
        }



        private void OnDestroy()
        {
            GameInitializer.OnGameEnd -= DeactivateSpawner;
        }



        [Serializable]
        public struct SpawnerInitData
        {
            [SerializeField] private float _spawnCoolDownSeconds;
            [SerializeField] private float _spawnTimeDeviationSeconds;

            [SerializeField] private WarrioirsPickStrategy _warrioirsPickStrategy;
            [SerializeField] private List<Warrior> _warriosToSpawn;



            public float SpawnCoolDownSeconds => _spawnCoolDownSeconds;
            public float SpawnTimeDeviationSeconds => _spawnTimeDeviationSeconds;
            public IReadOnlyList<Warrior> WarriosToSpawn => _warriosToSpawn;
            public WarrioirsPickStrategy WarrioirsPickStrategy => _warrioirsPickStrategy;
        }
    }
}
