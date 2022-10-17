using System;
using System.Collections.Generic;
using InfinityGame.Buildings;
using InfinityGame.Arena;
using InfinityGame.Fractions;
using InfinityGame.CashedData;
using UnityEngine;

internal class GameInitializer : MonoBehaviour
{
    public static event Action OnGameEnd;

    private static GameInitializer _instance;

    [SerializeField] private List<SpawnPlace> _spawnPlaces = new List<SpawnPlace>(4);
    [SerializeField] private List<Fraction> _fractions = new List<Fraction>(2);

    [SerializeField] private Building _buildingPrefab;

/*    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _projectileTarget;*/


    /// <summary>
    /// Asseblies all fraction settings for game
    /// </summary>
    /// <param name="fraction"></param>
    /// <param name="spawnPlace"></param>
    /// <returns>List of all barracks of fraction</returns>
    private void AssembleFraction(Fraction fraction, SpawnPlace spawnPlace, ref Action onArenaAssebmlyEnd)
    {
        // TODO: ־עהוכüםûי לועמה
        var townHall = Building.Instantiate(_buildingPrefab, fraction.Tag, fraction.TownHallBuildingData);
        var townHallSpawner = townHall.gameObject.AddComponent<WarrioirSpawner>();
        townHall.transform.position = spawnPlace.TownHallSpawnPointPosition;
        townHall.name = $"TownHall {fraction.Tag}";

        onArenaAssebmlyEnd += () => townHallSpawner.Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

        GameCasher.CashBuilding(townHall);
        townHall.OnDie += () => GameCasher.UncashBuilding(townHall);

        // Set all barracks on positions
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = Building.Instantiate(_buildingPrefab, fraction.Tag, fraction.BarrackBuildingData);
            var barrackSpawner = barrack.gameObject.AddComponent<WarrioirSpawner>();
            barrack.transform.position = barrackPosition;
            barrack.name = $"Barrack {fraction.Tag} {barrack.transform.position}";

            GameCasher.CashBuilding(barrack);
            onArenaAssebmlyEnd += () => barrackSpawner.Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

            townHall.OnDie += () =>
            {
                GameCasher.UncashBuilding(barrack);
                Destroy(barrack.gameObject); // If townhall dies - then all barrack get destroyed too
            };
        }
    }

    private IList<int> GetReservedSpawnPlaceIndexes()
    {
        var indexes = new List<int>(_spawnPlaces.Capacity);

        foreach (var spawnPlace in _spawnPlaces)
            indexes.Add(_spawnPlaces.IndexOf(spawnPlace));

        return indexes;
    }

    private void AssembleArena()
    {
        Action onAreanaAssemblyEnd = null;

        var emptySpawnPlaceIndexes = GetReservedSpawnPlaceIndexes();
        var possibleToGenerateFractions = new List<Fraction>(_fractions);

        while (possibleToGenerateFractions.Count != 0 && emptySpawnPlaceIndexes.Count != 0)
        {
            var randomIndexOfFraction = StaticNumberOperator.Randomizer.Next(0, possibleToGenerateFractions.Count);
            var radnomIndexOfPlace = StaticNumberOperator.Randomizer.Next(0, emptySpawnPlaceIndexes.Count);

            AssembleFraction(possibleToGenerateFractions[randomIndexOfFraction], _spawnPlaces[emptySpawnPlaceIndexes[radnomIndexOfPlace]], ref onAreanaAssemblyEnd);

            possibleToGenerateFractions.RemoveAt(randomIndexOfFraction);
            emptySpawnPlaceIndexes.RemoveAt(radnomIndexOfPlace);
        }

        // Invoke all preparation, after full map spawn 
        onAreanaAssemblyEnd?.Invoke();
    }


    private void Awake()
    {
        #region [Singleton]

        if (_instance != null)
            Destroy(gameObject);

        _instance = this; 

        #endregion

        AssembleArena();

        // TEST

 /*       var testProjectile = Instantiate(_projectilePrefab);
        testProjectile.Throw(_projectileTarget);*/
    }

    private void OnApplicationQuit()
    {
        OnGameEnd?.Invoke();
    }
}
