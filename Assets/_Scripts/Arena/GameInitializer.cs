using System.Collections.Generic;
using System;
using InfinityGame.Factories.BuildingFactory;
using InfinityGame.GameEntities;
using InfinityGame.DataCaching;
using InfinityGame.Fractions;
using InfinityGame.Arena;
using UnityEngine;

internal class GameInitializer : MonoBehaviour
{
    public static Action OnGameEnd;
    private static GameInitializer _instance;

    [SerializeField] private List<SpawnPlace> _spawnPlaces = new List<SpawnPlace>(4);
    [SerializeField] private List<Fraction> _fractions = new List<Fraction>(2);

    private BuildingFactory _buildingFactory;



    /// <summary>
    /// Asseblies all fraction settings for game
    /// </summary>
    /// <param name="fraction"></param>
    /// <param name="spawnPlace"></param>
    /// <returns>List of all barracks of fraction</returns>
    private void AssembleFraction(Fraction fraction, SpawnPlace spawnPlace)
    {
        var townHallObject = new GameObject(fraction.TownHallBuildingData.Name);
        var townHall = townHallObject.AddComponent<TownHall>();

        FractionCacher.CashFraction(fraction, townHall);

        var townHallBarrack = _buildingFactory.SpawnFractionBuilding<Barrack>(fraction, fraction.TownHallBuildingData, spawnPlace.TownHallSpawnPointPosition);
        townHall.SetBarrack(townHallBarrack);

        // Spawn all sub barracks
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
            _buildingFactory.SpawnFractionBuilding<Barrack>(fraction, fraction.BarrackBuildingData, barrackPosition);
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
        _buildingFactory = new BuildingFactory();

        var emptySpawnPlaceIndexes = GetReservedSpawnPlaceIndexes();
        var possibleToGenerateFractions = new List<Fraction>(_fractions);

        while (possibleToGenerateFractions.Count != 0 && emptySpawnPlaceIndexes.Count != 0)
        {
            var randomIndexOfFraction = StaticRandomizer.Randomizer.Next(0, possibleToGenerateFractions.Count);
            var radnomIndexOfPlace = StaticRandomizer.Randomizer.Next(0, emptySpawnPlaceIndexes.Count);

            AssembleFraction(possibleToGenerateFractions[randomIndexOfFraction],
                _spawnPlaces[emptySpawnPlaceIndexes[radnomIndexOfPlace]]);

            possibleToGenerateFractions.RemoveAt(randomIndexOfFraction);
            emptySpawnPlaceIndexes.RemoveAt(radnomIndexOfPlace);
        }
    }

    private void EndGame() => OnGameEnd?.Invoke();



    private void Awake()
    {
        #region [Singleton]

        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        #endregion

        FractionCacher.OnOneFractionLeft += EndGame;

        AssembleArena();
    }
}
