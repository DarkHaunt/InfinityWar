using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.GameEntities;
using InfinityGame.Spawning;
using InfinityGame.Fractions;

namespace InfinityGame.DataCaching
{
    using FractionType = FractionHandler.FractionType;

    public static class FractionCacher
    {
        public static event Action OnGameEnd; // TODO: По сути тут этому не место

        private static readonly Dictionary<FractionType, FractionGameData> _cachedFractions = new Dictionary<FractionType, FractionGameData>();



        public static void CashFraction(Fraction fraction, TownHall townHall)
        {
            if (IsFractionCached(fraction.FractionType))
                throw new UnityException($"{fraction} is already cashed, but you're trying to cash it again.");

            var fractionCashedData = new FractionGameData(fraction, townHall);
            _cachedFractions.Add(fraction.FractionType, fractionCashedData);

            fractionCashedData.OnFractionLose += () => UncacheFractionData(fractionCashedData);
        }

        private static void UncacheFractionData(FractionGameData fractionCashedData)
        {
            if (!IsFractionCached(fractionCashedData.Fraction))
                throw new UnityException($"{fractionCashedData.Fraction} doesn't conatin in cashe, and you're trying uncash it");

            _cachedFractions.Remove(fractionCashedData.Fraction);

            if (_cachedFractions.Count <= 1)
                OnGameEnd?.Invoke();
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

        public static FractionGameData GetFractionCachedData(FractionType fractionType)
        {
            if (!IsFractionCached(fractionType))
                throw new UnityException($"Fraction Cahser doesn't contain fraction {fractionType}");

            return _cachedFractions[fractionType];
        }

        public static IEnumerable<GameEntity> GetEnemyEntitiesOfFraction(FractionType fractionType)
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

        private static bool IsFractionCached(FractionType fractionType) => _cachedFractions.ContainsKey(fractionType);



        /// <summary>
        /// Contains data abount fraction , which needed for game
        /// </summary>
        public class FractionGameData
        {
            public event Action OnFractionLose;
            public event Action OnWarrioirLimitOverflow;
            public event Action OnWarrioirLimitRelease;

            private readonly int WarrioirCountLimit;
            private int _warriorCount = 0;

            private TownHall _townHall;
            private readonly HashSet<Warrior> _warriors;

            private bool _isWarrioirLimitOverflowed = false;
            private bool _townHallIsDead = false;



            public IEnumerable<GameEntity> GetEntitiesForTargeting
            {
                get
                {
                    if (_townHallIsDead)
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


                    yield return _townHall; // Because town hall is a fraction building too
                }

            }

            public FractionType Fraction { get; }



            public FractionGameData(Fraction fraction, TownHall fractionTownHall)
            {
                WarrioirCountLimit = fraction.WarrioirMaxLimit;
                Fraction = fraction.FractionType;

                _warriors = new HashSet<Warrior>();

                _townHall = fractionTownHall;
                _townHall.OnZeroHealth += OnTownHallDeath;
            }



            public void CacheBuilding(Building building)
            {
                if (_townHallIsDead)
                    throw new UnityException($"Townhall fo fraction {Fraction} has been destroyed, so you can't cache buildings of fraction anymore");

                _townHall.AddBuilding(building);
            }

            public void UncacheBuilding(Building building)
            {
/*                if (_townHallIsDead)
                    throw new UnityException($"Townhall fo fraction {Fraction} has been destroyed, so you can't uncache buildings of fraction anymore");*/

                _townHall.RemoveBuilding(building);
            }

            private void CheckForLose()
            {
                if (_townHallIsDead && _warriors.Count == 0)
                    OnFractionLose?.Invoke();
            }

            public void CacheWarrior(Warrior warrior)
            {
                if (!_warriors.Add(warrior))
                    throw new UnityException($"{warrior} warrior is already cashed, but you're trying to cash it again.");

                IncreaseWarrioirCount();
            }

            public void UncacheWarrior(Warrior warrior)
            {
                if (!_warriors.Remove(warrior))
                    throw new UnityException($"{warrior} warrior is not cashed, but you're trying to uncash it.");

                DecreaseWarriorCount();

                if (_townHallIsDead)
                    CheckForLose();
            }

            private void IncreaseWarrioirCount()
            {
                _warriorCount++;

                if (_warriorCount >= WarrioirCountLimit)
                {
                    _isWarrioirLimitOverflowed = true;
                    OnWarrioirLimitOverflow?.Invoke();
                }
            }

            private void DecreaseWarriorCount()
            {
                _warriorCount--;

                if (_isWarrioirLimitOverflowed && _warriorCount < WarrioirCountLimit)
                {
                    _isWarrioirLimitOverflowed = false;
                    OnWarrioirLimitRelease?.Invoke();
                }
            }

            private void OnTownHallDeath()
            {
                _townHallIsDead = true;
                CheckForLose();
            }
        }
    }
}
