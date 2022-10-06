using System.Collections;
using System;
using System.Collections.Generic;
using InfinityGame.Buildings;
using InfinityGame.Arena;
using UnityEngine;

internal class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    private event Action OnGenerationEnd;

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
        var townHall = TownHall.Instantiate(_townHallPrefab, fraction, OnGenerationEnd);

        // Set townhall outside data
        townHall.transform.position = spawnPlace.TownHallSpawnPointPosition;
        townHall.OnDie += () => _townHalls.Remove(townHall);
        // Manipulations for collection
        _townHalls.Add(townHall);

        // Set all barracks on positions
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = Barrack.Instantiate(_barrackPrefab, fraction, OnGenerationEnd);
            barrack.transform.position = barrackPosition;

            townHall.OnDie += () => Destroy(barrack.gameObject); // If townhall dies - then all barrack get destroyed too
        }
    }

    private void SetNewGlobalTargetToWarrioir(Warrior warrior)
    {
        HitableEntity target = null;
        var minimalDiscoveredDistance = float.MaxValue;

        for (int i = 0; i < _townHalls.Count; i++)
        {
            // Ignore own townhall
            if (_townHalls[i].FractionTag == warrior.FractionTag)
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



    private void Awake()
    {
        if (_instance != null)
            Destroy(gameObject);

        _instance = this;

        var randomizer = new System.Random();

        // For random placement
        var indexes = new List<int>(_spawnPlaces.Capacity);

        foreach (var spawnPlace in _spawnPlaces)
            indexes.Add(_spawnPlaces.IndexOf(spawnPlace));

        var nonGeneratedFractions = new List<Fraction>(_fractions);

        // Do while there is no fraction or there is no any places
        while (nonGeneratedFractions.Count != 0 && indexes.Count != 0)
        {
            var randomIndexOfFraction = randomizer.Next(0, nonGeneratedFractions.Count);
            var radnomIndexOfPlace = randomizer.Next(0, indexes.Count);

            AssembleFraction(nonGeneratedFractions[randomIndexOfFraction], _spawnPlaces[indexes[radnomIndexOfPlace]]);

            nonGeneratedFractions.RemoveAt(randomIndexOfFraction);
            indexes.RemoveAt(radnomIndexOfPlace);
        }

        // Invoke all preparation, after full map spawn 
        OnGenerationEnd?.Invoke();

        Warrior.OnWarriorTargetDeath += SetNewGlobalTargetToWarrioir;
    }
}
