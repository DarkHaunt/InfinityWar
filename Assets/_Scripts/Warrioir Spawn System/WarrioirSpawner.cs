using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using InfinityGame.Strategies.WarrioirSpawnStrategies;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.GameEntities;
using InfinityGame.DataCaching;


/// <summary>
/// Loop spawning certain count of warriors determied by spawn strategy
/// </summary>
public class WarrioirSpawner : MonoBehaviour
{
    // A max scatter between spawn position and spawner position in units
    private static Vector2 MaxSpawnPositionScatter = new Vector2(1f, 1f);

    private float _spawnCoolDownSeconds;
    private float _spawnDelaySeconds;

    private List<Warrior> _warriorsToSpawn;
    private CancellationTokenSource _spawnCanceller;
    private WarrioirsPickStrategy _warriorsPickStrategy;



    public void Initialize(string fractionTag, SpawnData spawnData, WarrioirsPickStrategy warriorPickStrategy)
    {
        _spawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
        _spawnDelaySeconds = spawnData.TimeDeltaSeconds;

        if (_spawnDelaySeconds >= _spawnCoolDownSeconds)
            throw new UnityException($"Spawn delay can't be equal or higher than cool down seconds!");

        _warriorsToSpawn = spawnData.WarriosToSpawn;

        _warriorsPickStrategy = warriorPickStrategy;
        _spawnCanceller = new CancellationTokenSource();

        FractionCacher.OnGameEnd += _spawnCanceller.Cancel;

        var cashedFraction = FractionCacher.TryToGetFractionCashedData(fractionTag);
        cashedFraction.OnWarrioirLimitRelease += StartSpawn;
        cashedFraction.OnWarrioirLimitOverflow += CancelSpawning;

        StartSpawn();
    }

    private async void StartSpawn()
    {
        while (true)
        {
            var taskOfGettingWave = Task.Run(() => _warriorsPickStrategy.ChoseWarrioirsToSawn(_warriorsToSpawn), _spawnCanceller.Token);
            var coolDownTask = Task.Delay(NewSpawnDelayMiliseconds(), _spawnCanceller.Token);

            try
            {
                await Task.WhenAll(taskOfGettingWave, coolDownTask);
            }
            catch (Exception e)
            {
                if (_spawnCanceller.IsCancellationRequested)
                    break;
                else
                    Debug.Log(e.Message);
            }

            var warriorPrefabs = taskOfGettingWave.Result;

            foreach (var warrioirPrefab in warriorPrefabs)
            {
                if (_spawnCanceller.IsCancellationRequested)
                {
                    RefreshCancellationToken();
                    return;
                }

                var warrior =  WarriorFactory.InstantiateWarrior(warrioirPrefab);
                warrior.transform.position = transform.position + RandomPositionDeviation();

                await Task.Delay(1000);
            }
        }

        RefreshCancellationToken();
    }

    private void RefreshCancellationToken()
    {
        _spawnCanceller.Dispose();
        _spawnCanceller = new CancellationTokenSource();
    }

    private void CancelSpawning() => _spawnCanceller.Cancel();

    private Vector3 RandomPositionDeviation()
    {
        var randomX = (float)(StaticRandomizer.GetRandomSign() * StaticRandomizer.Randomizer.NextDouble() * MaxSpawnPositionScatter.x);
        var randomY = (float)(StaticRandomizer.GetRandomSign() * StaticRandomizer.Randomizer.NextDouble() * MaxSpawnPositionScatter.y);

        return new Vector3(randomX, randomY);
    }

    private int NewSpawnDelayMiliseconds()
    {
        var randomPercentOfDeltaMiliseconds = StaticRandomizer.Randomizer.NextDouble();
        var rawTime = (_spawnCoolDownSeconds + (StaticRandomizer.GetRandomSign() * _spawnDelaySeconds * randomPercentOfDeltaMiliseconds)) * 1000;

        return (int)rawTime;
    }



    private void OnDestroy()
    {
        _spawnCanceller.Cancel();
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
