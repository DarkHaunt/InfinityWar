using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.GameEntities;
using InfinityGame.Strategies.WarrioirSpawnStrategies;


public class WarrioirSpawner : MonoBehaviour
{
    // A scratter between spawn position and spawner in units
    private static Vector2 SpawnPositionScatter = new Vector2(1f, 1f);

    private float _generalSpawnCoolDownSeconds;
    private float _spawnTimeDeltaSeconds;

    private List<Warrior> _warriorsToSpawn;
    private CancellationTokenSource _spawnCanceller;
    private WarrioirsPickStrategy _warriorsPickStrategy;


    public void Initialize(SpawnData spawnData, WarrioirsPickStrategy warrioirChoseStrategy)
    {
        _generalSpawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
        _spawnTimeDeltaSeconds = spawnData.TimeDeltaSeconds;
        _warriorsToSpawn = spawnData.WarriosToSpawn;

        _warriorsPickStrategy = warrioirChoseStrategy;
        _spawnCanceller = new CancellationTokenSource();

        GameInitializer.OnGameEnd += _spawnCanceller.Cancel;
        StartGeneration();
    }

    private async void StartGeneration()
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
                    return;
                else
                    Debug.Log(e.Message);
            }

            var warriorPrefabs = taskOfGettingWave.Result;

            foreach (var warrioirPrefab in warriorPrefabs)
            {
                var warrioir = WarriorFactory.InstantiateWarrior(warrioirPrefab);
                warrioir.transform.position = transform.position + RandomPositionDeviation();
            }
        }
    }

    private Vector3 RandomPositionDeviation()
    {
        var randomX = (float)(StaticNumberOperator.GetRandomSign() * StaticNumberOperator.Randomizer.NextDouble() * SpawnPositionScatter.x);
        var randomY = (float)(StaticNumberOperator.GetRandomSign() * StaticNumberOperator.Randomizer.NextDouble() * SpawnPositionScatter.y);

        return new Vector3(randomX, randomY);
    }

    private int NewSpawnDelayMiliseconds()
    {
        var randomPercentOfDeltaMiliseconds = StaticNumberOperator.Randomizer.NextDouble();
        var rawTime = (_generalSpawnCoolDownSeconds + (StaticNumberOperator.GetRandomSign() * _spawnTimeDeltaSeconds * randomPercentOfDeltaMiliseconds)) * 1000;

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
