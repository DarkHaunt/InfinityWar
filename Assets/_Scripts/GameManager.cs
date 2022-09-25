using System.Collections;
using System.Collections.Generic;
using InfinityGame.Buildings;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Layers of sides
    private static GameManager _instance;

    [SerializeField] private List<SpawnPlace> _spawnPlaces = new List<SpawnPlace>(4);
    [SerializeField] private List<Fraction> _fractions = new List<Fraction>(2);

    [SerializeField] private TownHall _townHallPrefab;
    [SerializeField] private Barrack _barrackPrefab;


    private void AssembleFraction(Fraction fraction, SpawnPlace spawnPlace)
    {
        var townHall = TownHall.Instantiate(_townHallPrefab, fraction);
        // Set townHall on position
        townHall.transform.position = spawnPlace.TownHallSpawnPointPosition;

        // Set all barracks on positions
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = Barrack.Instantiate(_barrackPrefab, fraction);
            barrack.transform.position = barrackPosition;
        }    
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

        while (nonGeneratedFractions.Count != 0 && indexes.Count != 0)
        {
            var randomIndexOfFraction = randomizer.Next(0, nonGeneratedFractions.Count);
            var radnomIndexOfPlace = randomizer.Next(0, indexes.Count);

            AssembleFraction(nonGeneratedFractions[randomIndexOfFraction], _spawnPlaces[indexes[radnomIndexOfPlace]]);

            nonGeneratedFractions.RemoveAt(randomIndexOfFraction);
            indexes.RemoveAt(radnomIndexOfPlace);
        }


/*        foreach (var spawnPlace in _spawnPlaces)
        {
            var index = randomizer.Next(0, nonGeneratedFractions.Count);

            AssembleFraction(nonGeneratedFractions[index], spawnPlace);
            nonGeneratedFractions.RemoveAt(index);

            if (nonGeneratedFractions.Count == 0)
                break;
        }*/
            

    }


}
