using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;
using InfinityGame.Factories.WarriorFactory;

public class WarrioirSpawner : MonoBehaviour
{
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

        StartGeneration();
    }

    private async void StartGeneration()
    {
        while (true)
        {
            var taskOfGettingWave = Task.Run(() => _warriorsPickStrategy.ChoseWarrioirsToSawn(_warriorsToSpawn), _spawnCanceller.Token);

            var coolDownTask = Task.Delay(GetNewSpawnDelayMiliseconds(), _spawnCanceller.Token);

            try
            {
                await Task.WhenAll(taskOfGettingWave, coolDownTask);
            }
            catch(Exception e)
            {
                if (_spawnCanceller.IsCancellationRequested)
                    return;
                else
                    Debug.Log(e.Message);
            }

            await Task.Run(() =>
            {
                var warriorPrefabs = taskOfGettingWave.Result;

                foreach (var warrioirPrefab in warriorPrefabs)
                {
                    var warrioir = WarriorFactory.InstantiateWarrior(warrioirPrefab);
                    warrioir.transform.position = transform.position;
                }

            }, _spawnCanceller.Token);
        }
    }

    private int GetNewSpawnDelayMiliseconds()
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
