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
    public static readonly Dictionary<IWarrioirChoseStrategy.SpawnType, Func<IWarrioirChoseStrategy>> GetStrategyByType = new Dictionary<IWarrioirChoseStrategy.SpawnType, Func<IWarrioirChoseStrategy>>()
    {
/*        [IWarrioirChoseStrategy.SpawnType.RandomGroup] = new Func<IWarrioirChoseStrategy>(() =>
        {
            return new ChoseRandomGroup();
        }),*/

        [IWarrioirChoseStrategy.SpawnType.RandomSingle] = new Func<IWarrioirChoseStrategy>(() =>
        {
            return new ChoseRandomSingle();
        })
    };


    public static readonly int[] Sign = new int[2]
    {
        1,
        -1
    };

    // System layers for overlaps and other stuff

    public const string WarrioirsLayerName = "Warriors";
    public const string BuildingsLayerName = "Building";
}
