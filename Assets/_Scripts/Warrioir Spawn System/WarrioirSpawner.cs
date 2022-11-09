using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine;
using InfinityGame.Strategies.WarrioirPickStrategies;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.GameEntities;
using InfinityGame.DataCaching;
using InfinityGame.Fractions;



namespace InfinityGame.Spawning
{
    /// <summary>
    /// Loop spawning certain count of warriors determied by spawn strategy
    /// </summary>
    public class WarrioirSpawner : MonoBehaviour
    {
        public event Action OnSpawnerDeactivate;

        // A max scatter between spawn position and spawner position in units
        private static Vector2 MaxSpawnPositionScatter = new Vector2(1f, 1f);

        private float _spawnCoolDownSeconds;
        private float _spawnDelaySeconds;

        private IReadOnlyList<Warrior> _warriorsToSpawn;
        private WarrioirsPickStrategy _warriorsPickStrategy;
        private string _fractionTag;

        private bool _isSpawning = true;



        public string Fraction => _fractionTag;
        


        public void Initialize(Fraction fraction) => Initialize(fraction.FractionTag, fraction.BarracksWarrioirSpawnSettings);

        public void Initialize(string fractionTag, SpawnData spawnData)
        {
            _spawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
            _spawnDelaySeconds = spawnData.TimeDeltaSeconds;

            _fractionTag = fractionTag;

            if (!IsAllWarrioirsBelongsToSpawnerFraction(spawnData.WarriosToSpawn))
                throw new UnityException($"Not all warrioirs belongs to fraction {_fractionTag} in spawner {gameObject.name}");

            if (_spawnDelaySeconds >= _spawnCoolDownSeconds)
                throw new UnityException($"Spawn delay can't be equal or higher than cool down seconds!");

            _warriorsToSpawn = spawnData.WarriosToSpawn;
            _warriorsPickStrategy = spawnData.WarrioirsPickStrategy;

            GameInitializer.OnGameEnd += StopSpawning;

            FractionCacher.TieUpSpawnerToFraction(this);
            StartSpawning();
        }

        public void DeactivateSpawning()
        {
            OnSpawnerDeactivate?.Invoke();
        }

        public void StartSpawning()
        {
            _isSpawning = true;
            StartCoroutine(SpawnCoroutine());
        }

        public void StopSpawning()
        {
            _isSpawning = false;
        }

        private bool IsAllWarrioirsBelongsToSpawnerFraction(IEnumerable<Warrior> warriors)
        {
            foreach (var warrior in warriors)
                if (!warrior.IsBelongsToFraction(Fraction))
                    return false;

            return true;
        }

        private IEnumerator SpawnCoroutine()
        {
            while (_isSpawning)
            {
                yield return new WaitForSeconds(NewSpawnDelaySeconds());

                var pickedWarioirsPrefabs = _warriorsPickStrategy.ChoseWarrioirsToSawn(_warriorsToSpawn);


                foreach (var warrioirPrefab in pickedWarioirsPrefabs)
                {
                    if (!_isSpawning)
                        break;

                    var warrioirPosition = transform.position + RandomPositionDeviation();
                    WarriorFactory.InstantiateWarrior(warrioirPrefab, warrioirPosition);
                }
            }
        }

        private Vector3 RandomPositionDeviation()
        {
            var randomX = (float)(StaticRandomizer.GetRandomSign() * StaticRandomizer.Randomizer.NextDouble() * MaxSpawnPositionScatter.x);
            var randomY = (float)(StaticRandomizer.GetRandomSign() * StaticRandomizer.Randomizer.NextDouble() * MaxSpawnPositionScatter.y);

            return new Vector3(randomX, randomY);
        }

        private float NewSpawnDelaySeconds()
        {
            var randomPercentOfDeltaMiliseconds = StaticRandomizer.Randomizer.NextDouble();
            var rawTime = (_spawnCoolDownSeconds + (StaticRandomizer.GetRandomSign() * _spawnDelaySeconds * randomPercentOfDeltaMiliseconds));

            return (float)rawTime;
        }



        private void OnDestroy()
        {
            GameInitializer.OnGameEnd -= DeactivateSpawning;
        }



        [Serializable]
        public struct SpawnData
        {
            [SerializeField] private float _spawnCoolDownSeconds;
            [SerializeField] private float _timeDeltaSeconds;

            [SerializeField] private WarrioirsPickStrategy _warrioirsPickStrategy;
            [SerializeField] private List<Warrior> _warriosToSpawn;



            public float SpawnCoolDownSeconds => _spawnCoolDownSeconds;

            public float TimeDeltaSeconds => _timeDeltaSeconds;

            public IReadOnlyList<Warrior> WarriosToSpawn => _warriosToSpawn;

            public WarrioirsPickStrategy WarrioirsPickStrategy => _warrioirsPickStrategy; 



            public SpawnData(float spawnCoolDownSeconds, float timeDeltaSeconds, List<Warrior> warriorsToSpawn, WarrioirsPickStrategy warrioirsPickStrategy)
            {
                _spawnCoolDownSeconds = spawnCoolDownSeconds;
                _timeDeltaSeconds = timeDeltaSeconds;
                _warriosToSpawn = warriorsToSpawn;
                _warrioirsPickStrategy = warrioirsPickStrategy;
            }
        }
    }
}
