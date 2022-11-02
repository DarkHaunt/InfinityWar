using System.Collections.Generic;
using InfinityGame.Fractions;
using InfinityGame.GameEntities;
using InfinityGame.Strategies.WarrioirSpawnStrategies;
using InfinityGame.DataCaching;
using UnityEngine;

/// <summary>
/// Spawner, which spawns only one fraction units
/// </summary>
public class FractionSpawner : WarrioirSpawner
{
    private FractionHandler.FractionType _fractionType;



    public void Initialize(FractionHandler.FractionType fractionType, SpawnData spawnData, WarrioirsPickStrategy warriorPickStrategy)
    {
        _fractionType = fractionType;

        if (!IsAllWarrioirsBelongsToSpawnerFraction(spawnData.WarriosToSpawn))
            throw new UnityException($"Not all warrioirs belongs to fraction {_fractionType} in spawner {gameObject.name}");

        // TODO: Решить тут проблему
/*        var fractionCachedData = FractionCacher.GetFractionCachedData(Fraction);
        fractionCachedData.OnWarrioirLimitRelease += _warriorSpawner.StartSpawning;
        fractionCachedData.OnWarrioirLimitOverflow += _warriorSpawner.StopSpawning;

        OnZeroHealth += () =>
        {
            fractionCachedData.OnWarrioirLimitRelease -= _warriorSpawner.StartSpawning;
            fractionCachedData.OnWarrioirLimitOverflow -= _warriorSpawner.StopSpawning;
        };*/


        Initialize(spawnData, warriorPickStrategy);
    }

    public void Initialize(Fraction fraction)
    {
        _fractionType = fraction.FractionType;

        if (!IsAllWarrioirsBelongsToSpawnerFraction(fraction.WarrioirSpawnSettings.WarriosToSpawn))
            throw new UnityException($"Not all warrioirs belongs to fraction {_fractionType} in spawner {gameObject.name}");


        Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);
    }

    private bool IsAllWarrioirsBelongsToSpawnerFraction(IEnumerable<Warrior> warriors)
    {
        foreach (var warrior in warriors)
            if (!warrior.IsBelongsToFraction(_fractionType))
                return false;

        return true;
    }

}
