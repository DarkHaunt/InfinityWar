using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using InfinityGame.Buildings;
using InfinityGame.SpawnStrategies;
using UnityEngine;



public interface IWarrioirSpawner : IFractionTagable
{
    public float NextSpawnTime { get; set; }
    public float SpawnTimeDelta { get; set; }
    public List<Warrior> WarrioisToSpawn { get; }


    CancellationTokenSource SpawnCanceller { get; set; }
    IWarrioirChoseStrategy SpawnStrategy { get; set; }


    void GenerateWarrioirsWaves();
}
