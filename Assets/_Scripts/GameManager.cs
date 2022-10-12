using System;
using System.Collections.Generic;
using InfinityGame.Buildings;
using InfinityGame.Arena;
using UnityEngine;

internal class GameManager : MonoBehaviour // TODO 2: При успешных пердыдущих шагах переименовать на GameInitializer
{
    private static GameManager _instance;
    public static event Action OnGenerationEnd;
    public static event Action OnGameEnd;

    [SerializeField] private List<SpawnPlace> _spawnPlaces = new List<SpawnPlace>(4);
    [SerializeField] private List<Fraction> _fractions = new List<Fraction>(2);

    [SerializeField] private TownHall _townHallPrefab;
    [SerializeField] private Barrack _barrackPrefab;

    
    private List<TownHall> _townHalls = new List<TownHall>();


    /// <summary>
    /// Asseblies all fraction settings for game
    /// </summary>
    /// <param name="fraction"></param>
    /// <param name="spawnPlace"></param>
    /// <returns>List of all barracks of fraction</returns>
    private void AssembleFraction(Fraction fraction, SpawnPlace spawnPlace)
    {
        var townHall = TownHall.Instantiate(_townHallPrefab, spawnPlace.TownHallSpawnPointPosition, fraction); // TODO : OnGenerationEnd должен быть объявлен в методе AssembleArena и вызва в его конце, а не храниться в классе 
        townHall.OnDie += () => _townHalls.Remove(townHall);
        _townHalls.Add(townHall);

        // Set all barracks on positions
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = Barrack.Instantiate(_barrackPrefab, barrackPosition, fraction);
            townHall.OnDie += () => Destroy(barrack.gameObject); // If townhall dies - then all barrack get destroyed too
        }
    }

    // TODO 1 : Лишнее тут, попробовать убрать
    private void SetNewGlobalTargetToWarrioir(Warrior warrior)
    {
        FractionEntity target = null;
        var minimalDiscoveredDistance = float.MaxValue;

        for (int i = 0; i < _townHalls.Count; i++)
        {
            // Ignore own townhall
            if (_townHalls[i].IsSameFraction(warrior.FractionTag))
                continue;

            var distanceToCurrentTarget = Vector3.Distance(_townHalls[i].transform.position, warrior.transform.position);

            if (distanceToCurrentTarget < minimalDiscoveredDistance)
            {
                target = _townHalls[i];
                minimalDiscoveredDistance = distanceToCurrentTarget;
            }
        }

        warrior.SetNewGlobalTarget(target);
    }

    private IList<int> GetReservedSpawnPlaceIndexes()
    {
        var indexes = new List<int>(_spawnPlaces.Capacity);

        // TODO : Enumatator rework this
        foreach (var spawnPlace in _spawnPlaces)
            indexes.Add(_spawnPlaces.IndexOf(spawnPlace));

        return indexes;
    }

    private void AssembleArena()
    {
        Action onAreanaAssemblyEnd;

        var emptySpawnPlaceIndexes = GetReservedSpawnPlaceIndexes();
        var possibleToGenerateFractions = new List<Fraction>(_fractions);

        while (possibleToGenerateFractions.Count != 0 && emptySpawnPlaceIndexes.Count != 0)
        {
            var randomIndexOfFraction = StaticData.Randomizer.Next(0, possibleToGenerateFractions.Count);
            var radnomIndexOfPlace = StaticData.Randomizer.Next(0, emptySpawnPlaceIndexes.Count);

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

        Warrior.OnMainTargetSwitch += SetNewGlobalTargetToWarrioir;

        // Invoke all preparation, after full map spawn 
        OnGenerationEnd?.Invoke();
    }

    private void OnApplicationQuit()
    {
        OnGameEnd?.Invoke();
    }
}
