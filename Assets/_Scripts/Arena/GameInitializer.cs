using System.Collections.Generic;
using System;
using InfinityGame.Factories.BuildingFactory;
using InfinityGame.DataCaching;
using InfinityGame.Arena;
using InfinityGame.Fractions;
using UnityEngine;

internal class GameInitializer : MonoBehaviour
{
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
        FractionCacher.CashFraction(fraction);
        var townHall = _buildingFactory.CreateAndInitializeSpawnBuilding(fraction, spawnPlace.TownHallSpawnPointPosition, fraction.TownHallBuildingData);

        // Spawn all sub barracks
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = _buildingFactory.CreateAndInitializeSpawnBuilding(fraction, barrackPosition, fraction.BarrackBuildingData);
            barrack.OnZeroHealth += () => Destroy(barrack.gameObject);
        }

        townHall.OnZeroHealth += () =>
        {
            Action onAllBuildingsIterationEnd = null;

            foreach (var aliveFractionBuilding in FractionCacher.GetBuildingsOfFraction(townHall.FractionTag))
                onAllBuildingsIterationEnd += aliveFractionBuilding.Die;

            onAllBuildingsIterationEnd?.Invoke();
        };

        townHall.OnZeroHealth += () =>
        {
            Destroy(townHall.gameObject);
        };
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



    private void Awake()
    {
        #region [Singleton]

        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        #endregion

        AssembleArena();
    }
}
