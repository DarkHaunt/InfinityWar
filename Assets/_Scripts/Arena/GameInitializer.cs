using System.Collections.Generic;
using InfinityGame.GameEntities;
using InfinityGame.Factories.BuildingFactory;
using InfinityGame.CashedData;
using InfinityGame.Arena;
using InfinityGame.Fractions;
using UnityEngine;

internal class GameInitializer : MonoBehaviour
{
   // public static event Action OnGameEnd;

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
    private void AssembleFraction(Fraction fraction, SpawnPlace spawnPlace)
    {

        FractionCasher.CashFraction(fraction);
        // TODO: “х должен иметь в себе размер армии и считать, сколько войнов определЄнног типа сейчас на арене
        // ≈сли войнов такое же колличесвто, как и макс ращмер армии, то спавн любого война прекращаетс€
        // ѕосле уменьшени€ коллличества войнов - вновь работаетt
        var townHall = _buildingFactory.CreateSpawnBuilding(fraction, spawnPlace.TownHallSpawnPointPosition, fraction.TownHallBuildingData);

        // Spawn all sub barracks
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = _buildingFactory.CreateSpawnBuilding(fraction, barrackPosition, fraction.BarrackBuildingData);
            barrack.OnZeroHealth += () => Destroy(barrack.gameObject);
        }

        townHall.OnZeroHealth += () =>
        {
            var barracks = new List<FractionEntity>();

            foreach (var cashedBuilding in FractionCasher.GetAllyEntities(townHall.FractionTag))
                //if (cashedBuilding.IsSameFraction(townHall.FractionTag))
                barracks.Add(cashedBuilding);

            foreach (var barrack in barracks)
                barrack.Die();
        };

        townHall.OnZeroHealth += () =>
        {
            /*            if (BuildingCasher.IsOnlyOneFractionBuildingsLeft())
                            OnGameEnd?.Invoke();*/

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
        _buildingFactory = new BuildingFactory(_buildingPrefab);

        var emptySpawnPlaceIndexes = GetReservedSpawnPlaceIndexes();
        var possibleToGenerateFractions = new List<Fraction>(_fractions);

        while (possibleToGenerateFractions.Count != 0 && emptySpawnPlaceIndexes.Count != 0)
        {
            var randomIndexOfFraction = StaticRandomizer.Randomizer.Next(0, possibleToGenerateFractions.Count);
            var radnomIndexOfPlace = StaticRandomizer.Randomizer.Next(0, emptySpawnPlaceIndexes.Count);

            AssembleFraction(possibleToGenerateFractions[randomIndexOfFraction], _spawnPlaces[emptySpawnPlaceIndexes[radnomIndexOfPlace]]);

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

    private void OnApplicationQuit()
    {
       // OnGameEnd?.Invoke();
    }
}
