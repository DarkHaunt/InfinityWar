using System.Collections;
using System.Linq;
using System.Collections.Generic;
using InfinityGame.SpawnStrategies;
using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;


internal static class StaticData
{
    public static readonly Dictionary<ISpawnStrategy.SpawnType, Func<ISpawnStrategy.SpawnCycleData, ISpawnStrategy>> StrategiesRealization = new Dictionary<ISpawnStrategy.SpawnType, Func<ISpawnStrategy.SpawnCycleData, ISpawnStrategy>>()
    {
        [ISpawnStrategy.SpawnType.Group] = new Func<ISpawnStrategy.SpawnCycleData, ISpawnStrategy>((ISpawnStrategy.SpawnCycleData spawnData) =>
        {
            return new GroupSpawnStrategy(spawnData);
        }),

        [ISpawnStrategy.SpawnType.Single] = new Func<ISpawnStrategy.SpawnCycleData, ISpawnStrategy>((ISpawnStrategy.SpawnCycleData spawnData) =>
        {
            return new SingleSpawnStrategy(spawnData);
        }),
    };

    // System layers for overlaps and other stuff

    public const string WarrioirsLayerName = "Warriors";
    public const string BuildingsLayerName = "Building";
}
