using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.Strategies.WarrioirSpawnStrategies;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.GameEntities;
using InfinityGame.Fractions;
using InfinityGame.DataCaching;
using System.Collections;

/// <summary>
/// Loop spawning certain count of warriors determied by spawn strategy
/// </summary>
public class WarrioirSpawner : MonoBehaviour
{
    // A max scatter between spawn position and spawner position in units
    private static Vector2 MaxSpawnPositionScatter = new Vector2(1f, 1f);

    private float _spawnCoolDownSeconds;
    private float _spawnDelaySeconds;

    private IReadOnlyList<Warrior> _warriorsToSpawn;
    private WarrioirsPickStrategy _warriorsPickStrategy;

    private Coroutine _spawning;



    public void Initialize(Fraction fraction) => Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

    public void Initialize(SpawnData spawnData, WarrioirsPickStrategy warriorPickStrategy)
    {
        _spawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
        _spawnDelaySeconds = spawnData.TimeDeltaSeconds;

        if (_spawnDelaySeconds >= _spawnCoolDownSeconds)
            throw new UnityException($"Spawn delay can't be equal or higher than cool down seconds!");

        _warriorsToSpawn = spawnData.WarriosToSpawn;
        _warriorsPickStrategy = warriorPickStrategy;

        FractionCacher.OnGameEnd += StopSpawning;

        StartSpawning();
    }



    public void StartSpawning()
    {
        _spawning = StartCoroutine(SpawnCoroutine());
    }

    public void StopSpawning() => StopCoroutine(_spawning);

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(NewSpawnDelaySeconds());

            var pickedWarioirsPrefabs = _warriorsPickStrategy.ChoseWarrioirsToSawn(_warriorsToSpawn);


            foreach (var warrioirPrefab in pickedWarioirsPrefabs)
            {
                var warrior = WarriorFactory.InstantiateWarrior(warrioirPrefab);
                warrior.transform.position = transform.position + RandomPositionDeviation();
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
        FractionCacher.OnGameEnd -= StopSpawning;
    }



    [Serializable]
    public struct SpawnData
    {
        [SerializeField] private float _spawnCoolDownSeconds;
        [SerializeField] private float _timeDeltaSeconds;

        [SerializeField] private List<Warrior> _warriosToSpawn;


        public float SpawnCoolDownSeconds => _spawnCoolDownSeconds;

        public float TimeDeltaSeconds => _timeDeltaSeconds;

        public List<Warrior> WarriosToSpawn => _warriosToSpawn;


        public SpawnData(float spawnCoolDownSeconds, float timeDeltaSeconds, List<Warrior> warriorsToSpawn)
        {
            _spawnCoolDownSeconds = spawnCoolDownSeconds;
            _timeDeltaSeconds = timeDeltaSeconds;
            _warriosToSpawn = warriorsToSpawn;
        }
    }
}
