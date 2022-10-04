using System.Collections;
using System;
using System.Collections.Generic;
using InfinityGame.Buildings;
using InfinityGame.Arena;
using UnityEngine;

internal class GameManager : MonoBehaviour
{
    // Radius of overlapping for the all map
    //  public static readonly float OverlapRadius = 52.4f


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
    private List<IBarrack> AssembleFraction(Fraction fraction, SpawnPlace spawnPlace)
    {
        // All barracks for fraction
        var barracks = new List<IBarrack>(spawnPlace.BarracksSpawnPointsTransforms.Count + 1);

        var townHall = TownHall.Instantiate(_townHallPrefab, fraction, OnGenerationEnd);

        // Set townhall outside data
        townHall.transform.position = spawnPlace.TownHallSpawnPointPosition;
        townHall.OnDie += () => _townHalls.Remove(townHall);
        // Manipulations for collection
        _townHalls.Add(townHall);
        barracks.Add(townHall);

        // Set all barracks on positions
        foreach (var barrackPosition in spawnPlace.BarracksSpawnPointsTransforms)
        {
            var barrack = Barrack.Instantiate(_barrackPrefab, fraction, OnGenerationEnd);
            barrack.transform.position = barrackPosition;
            barrack.gameObject.tag = fraction.Tag;
            barracks.Add(barrack);
        }

        return barracks;
    }

    private void SetTargetsForAllTownHalls()
    {
        /*        for (int firstCycleIndex = 0; firstCycleIndex < _townHalls.Count; firstCycleIndex++)
                {
                    // If already targeted - move up to next
                    if (_townHalls[firstCycleIndex].IsTarget)
                        continue;

                    var distance = new DistanceBetweenTownHalls(); // Disance between two townhalls

                    for (int secondSycleIndex = 0; secondSycleIndex < _townHalls.Count; secondSycleIndex++)
                    {
                        // Sikp same townhall
                        if (firstCycleIndex == secondSycleIndex)
                            continue;

                        var currentDistanceBetweenTwoTownHalls = Vector3.Distance(_townHalls[firstCycleIndex].transform.position, _townHalls[secondSycleIndex].transform.position);

                        if (currentDistanceBetweenTwoTownHalls < distance.Distance)
                            distance.ChangeData(currentDistanceBetweenTwoTownHalls, firstCycleIndex, secondSycleIndex);

                    }



                }*/


    }

    /// <summary>
    /// Sets one of the enemy town halls as target for warriors for the barrack
    /// </summary>
    /// <param name="barrack"></param>
    private void SetTargetForBarrack(IBarrack barrack)
    {
        float minimalDistance = float.MaxValue;
        int indexOfTownHall = 0; // Index of closest townhall

        for (int i = 0; i < _townHalls.Count; i++)
        {
            var isNotATarget = _townHalls[i].FractionTag == barrack.FractionTag || _townHalls[i].transform.position == barrack.Position; // If not the same fraction object or not the same townhall
            // IF current barrack is one of the townhalls
            if (isNotATarget)
                continue;

            var currentDistance = Vector3.Distance(_townHalls[i].transform.position, barrack.Position);

            if (currentDistance < minimalDistance)
            {
                minimalDistance = currentDistance;
                indexOfTownHall = i;
            }
        }

        var newBarrackTarget = _townHalls[indexOfTownHall];
       // barrack.Target = newBarrackTarget.transform; // Set new target for current barrack

        var barrackSearchForNextTargetDelegate = new Action(() =>
        {
            SetTargetForBarrack(barrack); // After this target 
        });


        newBarrackTarget.OnDie += () => // If current target is dead - search for new one
        {
            barrackSearchForNextTargetDelegate?.Invoke();
            print($"Town hall {_townHalls[indexOfTownHall].transform.position} dies");
        };

/*        barrack.OnDie += () =>  // If barrack dies first - just unsubscribe from the event
        {
            print($"Barrack {barrack.Position} dies");
            newBarrackTarget.OnDie -= barrackSearchForNextTargetDelegate;
        };*/


        print($"For barrack {barrack.Position} nearliest is Townhall - {_townHalls[indexOfTownHall].transform.position}");
    }


    private void SetAllData()
    {
        var randomizer = new System.Random();

        // For random placement
        var indexes = new List<int>(_spawnPlaces.Capacity);


        foreach (var spawnPlace in _spawnPlaces)
            indexes.Add(_spawnPlaces.IndexOf(spawnPlace));

        // For random fraction
        var nonGeneratedFractions = new List<Fraction>(_fractions);


        while (nonGeneratedFractions.Count != 0 && indexes.Count != 0)
        {
            var randomIndexOfFraction = randomizer.Next(0, nonGeneratedFractions.Count);
            var radnomIndexOfPlace = randomizer.Next(0, indexes.Count);

            AssembleFraction(nonGeneratedFractions[randomIndexOfFraction], _spawnPlaces[indexes[radnomIndexOfPlace]]);

            nonGeneratedFractions.RemoveAt(randomIndexOfFraction);
            indexes.RemoveAt(radnomIndexOfPlace);
        }
    }


    private event Action OnHui;
    private ArguingTrigger trigger;

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

            var allFractionBarracks = AssembleFraction(nonGeneratedFractions[randomIndexOfFraction], _spawnPlaces[indexes[radnomIndexOfPlace]]);

            foreach (var barrack in allFractionBarracks)
                OnGenerationEnd += () =>
               {
                   SetTargetForBarrack(barrack);
                   //barrack.ChangeMainTarget(GetClosestTownHallTransform(barrack.transform.position));
               };
            //AssembleFraction(nonGeneratedFractions[randomIndexOfFraction], _spawnPlaces[indexes[radnomIndexOfPlace]]);

            nonGeneratedFractions.RemoveAt(randomIndexOfFraction);
            indexes.RemoveAt(radnomIndexOfPlace);
        }

        trigger = new GameObject("Test shit").AddComponent<ArguingTrigger>();


        OnHui += trigger.YaZalupa;
        // Invoke all preparation, after full map spawn 
        OnGenerationEnd?.Invoke();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OnHui?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.A))
            Destroy(trigger.gameObject);

    }


    public class DistanceBetweenTownHalls
    {
        private float _distance;
        private int _firstTownHallIndex;
        private int _secondTownHallIndex;


        public float Distance => _distance;
        public int FirstTownHallIndex => _firstTownHallIndex;
        public int SecondTownHallIndex => _secondTownHallIndex;


        public DistanceBetweenTownHalls(float distance, int firstIndex, int secondIndex)
        {
            _distance = distance;
            _firstTownHallIndex = firstIndex;
            _secondTownHallIndex = secondIndex;
        }

        public DistanceBetweenTownHalls()
        {
            _distance = float.MaxValue;
            _firstTownHallIndex = 0;
            _secondTownHallIndex = 1;
        }


        public void ChangeData(float distance, int firstIndex, int secondIndex)
        {
            _distance = distance;
            _firstTownHallIndex = firstIndex;
            _secondTownHallIndex = secondIndex;
        }
    }
}
