using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.GameEntities;
using InfinityGame.Spawning;
using InfinityGame.Fractions;

namespace InfinityGame.DataCaching
{
    public static class FractionCacher
    {
        public static event Action OnOneFractionLeft;

        private static readonly Dictionary<string, FractionGameData> _cachedFractions = new Dictionary<string, FractionGameData>();



        public static void CashFraction(Fraction fraction, TownHall fractionTownHall)
        {
            if (IsFractionCached(fraction.FractionTag))
                throw new UnityException($"{fraction} is already cashed, but you're trying to cash it again.");

            var fractionCashedData = new FractionGameData(fraction, fractionTownHall);
            _cachedFractions.Add(fraction.FractionTag, fractionCashedData);

            fractionCashedData.OnFractionLose += () => UncacheFractionData(fractionCashedData);
        }

        private static void UncacheFractionData(FractionGameData fractionCashedData)
        {
            if (!IsFractionCached(fractionCashedData.Fraction))
                throw new UnityException($"{fractionCashedData.Fraction} doesn't conatin in cashe, and you're trying uncash it");

            _cachedFractions.Remove(fractionCashedData.Fraction);

            if (_cachedFractions.Count == 1)
                OnOneFractionLeft?.Invoke();
        }

        public static void CacheBuilding(Building building)
        {
            if (!IsFractionCached(building.Fraction))
                throw new UnityException($"Fraction {building.Fraction} doesn't exist in cache, so building {building} can be cached");

            _cachedFractions[building.Fraction].CacheBuilding(building);
        }

        public static void UncacheBuilding(Building building)
        {
            if (!IsFractionCached(building.Fraction))
                throw new UnityException($"Fraction {building.Fraction} doesn't exist in cache, so building {building} can't be uncached");

            _cachedFractions[building.Fraction].UncacheBuilding(building);
        }

        public static void CacheWarrior(Warrior warrior)
        {
            if (!IsFractionCached(warrior.Fraction))
                throw new UnityException($"Fraction {warrior.Fraction} doesn't exist in cache, so warrior {warrior} can't be cached");

            _cachedFractions[warrior.Fraction].CacheWarrior(warrior);
        }

        public static void UncacheWarrior(Warrior warrior)
        {
            if (!IsFractionCached(warrior.Fraction))
                throw new UnityException($"Fraction {warrior.Fraction} doesn't exist in cache, so warrior {warrior} can't be uncached");

            _cachedFractions[warrior.Fraction].UncacheWarrior(warrior);
        }

        public static void TieUpSpawnerToFraction(WarrioirSpawner spawner)
        {
            if (!IsFractionCached(spawner.Fraction))
                throw new UnityException($"Fraction {spawner.Fraction} doesn't exist in cache, so spawner {spawner} can't be uncached");

            _cachedFractions[spawner.Fraction].PutSpawnerOnWarriorRecord(spawner);
        }

        public static IEnumerable<GameEntity> GetEnemyEntitiesOfFraction(string fractionType)
        {
            if (!IsFractionCached(fractionType))
                throw new UnityException($"Fraction {fractionType} doesn't exist in cache, so cacher can't take it's entities");


            foreach (var fractionData in _cachedFractions)
            {
                if (fractionData.Key == fractionType)
                    continue;

                foreach (var entity in fractionData.Value.GetEntitiesForTargeting)
                    yield return entity;
            }
        }

        private static bool IsFractionCached(string fractionTag) => _cachedFractions.ContainsKey(fractionTag);



        /// <summary>
        /// Contains data abount fraction , which needed for game
        /// </summary>
        public class FractionGameData
        {
            public event Action OnFractionLose;

            private readonly HashSet<Warrior> _warriors;
            private readonly TownHall _townHall;

            private bool _fractionHaveTownHall = false;

            private readonly Counter _warrioirCounter;



            public IEnumerable<GameEntity> GetEntitiesForTargeting
            {
                get
                {
                    if (!_fractionHaveTownHall)
                        return _warriors;

                    return Buildings;
                }
            }

            private IEnumerable<Building> Buildings
            {
                get
                {
                    foreach (var building in _townHall.Buildings)
                        yield return building;
                }

            }

            public string Fraction { get; }



            public FractionGameData(Fraction fraction, TownHall townHall)
            {
                _warrioirCounter = new Counter(fraction.WarrioirMaxLimit);
                Fraction = fraction.FractionTag;

                _warriors = new HashSet<Warrior>();

                _townHall = townHall;
                _fractionHaveTownHall = true;

                _townHall.OnDestroy += () =>
                {
                    _fractionHaveTownHall = false;
                    CheckForLose();
                };
            }



            public void CacheBuilding(Building building)
            {
                if (!_fractionHaveTownHall)
                    throw new UnityException($"Townhall fo fraction {Fraction} has been destroyed, so you can't cache buildings of fraction anymore");

                _townHall.AddBuilding(building);
            }

            public void UncacheBuilding(Building building)
            {
                _townHall.RemoveBuilding(building);
            }

            public void CacheWarrior(Warrior warrior)
            {
                if (!_warriors.Add(warrior))
                    throw new UnityException($"{warrior} warrior is already cashed, but you're trying to cash it again.");

                _warrioirCounter.Increase();
            }

            public void UncacheWarrior(Warrior warrior)
            {
                if (!_warriors.Remove(warrior))
                    throw new UnityException($"{warrior} warrior is not cashed, but you're trying to uncash it.");

                _warrioirCounter.Decrease();

                if (!_fractionHaveTownHall)
                    CheckForLose();
            }

            public void PutSpawnerOnWarriorRecord(WarrioirSpawner spawner)
            {
                _warrioirCounter.OnCounterLimitRelease += spawner.StartSpawning;
                _warrioirCounter.OnCounterLimitOverflow += spawner.StopSpawning;

                spawner.OnSpawnerDeactivate += () =>
                {
                    _warrioirCounter.OnCounterLimitRelease -= spawner.StartSpawning;
                    _warrioirCounter.OnCounterLimitOverflow -= spawner.StopSpawning;
                };
            }

            private void CheckForLose()
            {
                if (!_fractionHaveTownHall && _warriors.Count == 0)
                    OnFractionLose?.Invoke();
            }

        }
    }
}
