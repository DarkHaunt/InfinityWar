using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using InfinityGame.Buildings;
using InfinityGame.SpawnStrategies;
using UnityEngine;



public interface IBarrack : IFractionTagable, IGlobalTargetable
{
    Vector3 Position { get; }
    CancellationTokenSource CancellationSpawnTokenSource { get; set; }
    ISpawnStrategy SpawnStrategy { get; set; }

    
    

    void SpawnWave();
}
