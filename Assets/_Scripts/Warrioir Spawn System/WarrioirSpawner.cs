using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using InfinityGame.WarriorFactory;

public class WarrioirSpawner
{
    public event Action OnGeneration;

    private float _generalSpawnCoolDownSeconds;
    private float _spawnTimeDeltaSeconds;

    private List<Warrior> _warriorsToSpawn;
    private CancellationTokenSource _spawnCanceller;
    private WarrioirsPickStrategy _warriorsPickStrategy;
    private Vector2 _spawnerPosition;


    public CancellationTokenSource SpawnCanceller => _spawnCanceller;
    public Vector2 SpawnerPosition { set => _spawnerPosition = value; }


    public WarrioirSpawner(SpawnData spawnData, WarrioirsPickStrategy warrioirChoseStrategy)
    {
        _generalSpawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
        _spawnTimeDeltaSeconds = spawnData.TimeDeltaSeconds;
        _warriorsToSpawn = spawnData.WarriosToSpawn;

        _warriorsPickStrategy = warrioirChoseStrategy;
        _spawnCanceller = new CancellationTokenSource();

        GameManager.OnGameEnd += SpawnCanceller.Cancel;
    }


    public async void StartGeneration()
    {
        while (true)
        {
            var taskOfGettingWave = Task.Run(() => _warriorsPickStrategy.ChoseWarrioirsToSawn(_warriorsToSpawn), SpawnCanceller.Token);

            var coolDownTask = Task.Delay(GetNewSpawnDelayMiliseconds(), SpawnCanceller.Token);

            try
            {
                await Task.WhenAll(taskOfGettingWave, coolDownTask);
            }
            catch (OperationCanceledException e)
            {
                return;
            }
            catch (Exception e)
            {
                Debug.Log(e.Message);
            }


            var warriorPrefabs = taskOfGettingWave.Result;

            foreach (var warrioirPrefab in warriorPrefabs)
            {
                var warrioir = WarriorFactory.InstantiateWarriorOnPosition(warrioirPrefab);
                warrioir.transform.position = _spawnerPosition;
            }
        }
    }

    private int GetNewSpawnDelayMiliseconds()
    {
        var randomPercentOfDeltaMiliseconds = StaticData.Randomizer.NextDouble();
        var rawTime = (_generalSpawnCoolDownSeconds + (StaticData.GetRandomSign() * _spawnTimeDeltaSeconds * randomPercentOfDeltaMiliseconds)) * 1000;

        return (int)rawTime;
    }


    [Serializable]
    public struct SpawnData
    {
        public float SpawnCoolDownSeconds;
        public float TimeDeltaSeconds;

        public List<Warrior> WarriosToSpawn;

        public SpawnData(float spawnCoolDownSeconds, float timeDeltaSeconds, List<Warrior> warriorsToSpawn)
        {
            SpawnCoolDownSeconds = spawnCoolDownSeconds;
            TimeDeltaSeconds = timeDeltaSeconds;
            WarriosToSpawn = warriorsToSpawn;
        }
    }
}
