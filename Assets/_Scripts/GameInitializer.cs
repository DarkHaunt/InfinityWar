using System;
using System.Collections.Generic;
using InfinityGame.GameEntities;
using InfinityGame.Arena;
using InfinityGame.Fractions;
using InfinityGame.Factories.BuildingFactory;
using UnityEngine;

internal class GameInitializer : MonoBehaviour
{
    public static event Action OnGameEnd;

    private static GameInitializer _instance;

    [SerializeField] private List<SpawnPlace> _spawnPlaces = new List<SpawnPlace>(4);
    [SerializeField] private List<Fraction> _fractions = new List<Fraction>(2);

    [SerializeField] private Building _buildingPrefab;

    private BuildingFactory _buildingFactory;


    /// <summary>
    /// Asseblies all fraction settings for game
    /// </summary>
    /// <param name="fraction"></param>
    /// <param name="spawnPlace"></param>
    /// <returns>List of all barracks of fraction</returns>
    private void AssembleFraction(Fraction fraction, SpawnPlace spawnPlace, ref Action onArenaAssebmlyEnd)
    {
        // TODO: ־עהוכüםûי לועמה
        var fractionTownHallData = new Fraction.FractionBuildingData(fraction.Tag, $"TownHall {fraction.Tag}", fraction.TownHallBuildingData);
        var townHall = _buildingFactory.SpawnBuilding(fractionTownHallData, spawnPlace.TownHallSpawnPointPosition);

        var townHallSpawner = townHall.gameObject.AddComponent<WarrioirSpawner>();
        onArenaAssebmlyEnd += () => townHallSpawner.Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

        // Set all barracks on positions
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var fractionBarrackData = new Fraction.FractionBuildingData(fraction.Tag, $"Barrack {fraction.Tag} {barrackPosition}", fraction.BarrackBuildingData);
            var barrack = _buildingFactory.SpawnBuilding(fractionBarrackData, barrackPosition);

            var barrackSpawner = barrack.gameObject.AddComponent<WarrioirSpawner>();
            onArenaAssebmlyEnd += () => barrackSpawner.Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

            townHall.OnDie += () => barrack.Die();
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
        _buildingFactory = new BuildingFactory(_buildingPrefab);

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
    }

    private void OnApplicationQuit()
    {
        OnGameEnd?.Invoke();
    }
}
