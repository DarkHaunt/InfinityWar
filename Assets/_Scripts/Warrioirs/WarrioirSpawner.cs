using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;
using InfinityGame.Buildings;
using InfinityGame.SpawnStrategies;

public class WarrioirSpawner
{
    private Random _randomizer = new Random();

    private float _spawnCoolDownSeconds;
    private float _timeDeltaSeconds;

    private List<Warrior> _warriorsToSpawn;
    private CancellationTokenSource _spawnCanceller;
    private IWarrioirChoseStrategy _warriorsChoseStrategy;

    public CancellationTokenSource SpawnCanceller => _spawnCanceller;


    public WarrioirSpawner(SpawnData spawnData, IWarrioirChoseStrategy warrioirChoseStrategy)
    {
        _spawnCoolDownSeconds = spawnData.SpawnCoolDownSeconds;
        _timeDeltaSeconds = spawnData.TimeDeltaSeconds;
        _warriorsToSpawn = spawnData.WarriosToSpawn;

        _warriorsChoseStrategy = warrioirChoseStrategy;
        _spawnCanceller = new CancellationTokenSource();
    }


    public async void StartGeneration()
    {
        while (!SpawnCanceller.Token.IsCancellationRequested)
        {
            var taskOfGettingWave = new Task<IEnumerable<Warrior>>(() => _warriorsChoseStrategy.ChoseWarrioirsToSawn(_warriorsToSpawn), SpawnCanceller.Token);

            // TODO: Make a random delta of time
            var coolDownInMiliceconds = (int)(_spawnCoolDownSeconds + (StaticData.Sign[_randomizer.Next(0, StaticData.Sign.Length)] * _timeDeltaSeconds)) * 1000; 

            var coolDownTask = Task.Delay(coolDownInMiliceconds);


            await Task.WhenAll(taskOfGettingWave, coolDownTask); // Wait for cooldown and for warrioris get

            var warriors = taskOfGettingWave.Result;

            foreach (var warrior in warriors)
               UnityEngine.MonoBehaviour.Instantiate(warrior);
        }
    }

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
