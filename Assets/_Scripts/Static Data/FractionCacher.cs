using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.GameEntities;
using InfinityGame.Fractions;

namespace InfinityGame.DataCaching
{
    using FractionType = FractionHandler.FractionType;

    public static class FractionCacher // TODO: ������� ���� ���������������
    {
        public static event Action OnGameEnd; // TODO: �� ���� ��� ����� �� �����

        private static readonly Dictionary<FractionType, CachedFraction> _cachedFractions = new Dictionary<FractionType, CachedFraction>();



        public static IEnumerable<GameEntity> GetEnemyEntitiesOfFraction(FractionType fractionTag)
        {
            foreach (var fractionData in _cachedFractions)
            {
                if (fractionData.Key == fractionTag)
                    continue;

                foreach (var entity in fractionData.Value.GetEntitiesForTargeting)
                    yield return entity;
            }
        }

        public static void CashFraction(Fraction fraction)
        {
            if (IsFractionCached(fraction.FractionType))
                throw new UnityException($"{fraction} is already cashed, but you're trying to cash it again.");

            var fractionCashedData = new CachedFraction(fraction);
            _cachedFractions.Add(fraction.FractionType, fractionCashedData);

            fractionCashedData.OnFractionLose += () => UncacheFractionData(fractionCashedData);
        }

        private static void UncacheFractionData(CachedFraction fractionCashedData)
        {
            if (!_cachedFractions.ContainsKey(fractionCashedData.Fraction))
                throw new UnityException($"{fractionCashedData.Fraction} doesn't conatin in cashe, and you're trying uncash it");

            _cachedFractions.Remove(fractionCashedData.Fraction);

            if (_cachedFractions.Count <= 1)
                OnGameEnd?.Invoke();
        }

        public static void CacheBuilding(Building building)
        {
            if (!IsFractionCached(building.Fraction))
                return;

            _cachedFractions[building.Fraction].CacheBuilding(building);
        }

        public static void UncacheBuilding(Building building)
        {
            _cachedFractions[building.Fraction].UncacheBuilding(building);
        }

        public static void CacheWarrior(Warrior warrior)
        {
            if (!IsFractionCached(warrior.Fraction))
                return;

            _cachedFractions[warrior.Fraction].CacheWarrior(warrior);
        }

        public static void UncacheWarrior(Warrior warrior)
        {
            _cachedFractions[warrior.Fraction].UncacheWarrior(warrior);
        }

        public static CachedFraction TryToGetFractionCachedData(FractionType fractionTag)
        {
            if (IsFractionCached(fractionTag))
                return _cachedFractions[fractionTag];

            throw new UnityException($"Fraction Cahser doesn't contain fraction {fractionTag}");
        }

        private static bool IsFractionCached(FractionType fractionTag) => _cachedFractions.ContainsKey(fractionTag);



        public class CachedFraction
        {
            public event Action OnFractionLose;
            public event Action OnWarrioirLimitOverflow;
            public event Action OnWarrioirLimitRelease;

            private readonly int WarrioirCountLimit;
            private int _warriorCount = 0;

            private TownHall _townHall;
            private readonly HashSet<Warrior> _warriors;

            private bool _isWarrioirLimitOverflow = false;
            private bool _townHallIsDead = false;



            public IEnumerable<GameEntity> GetEntitiesForTargeting
            {
                get
                {
                    if (_townHallIsDead)
                        return _warriors;

                    return GetBuildings;
                }
            }

            public IEnumerable<Building> GetBuildings
            {
                get
                {
                    foreach (var building in _townHall.Buildings)
                        yield return building;


                    yield return _townHall; // Because town hall is a fraction building too
                }

            }

            public FractionType Fraction { get; }

            public TownHall TownHall
            {
                set
                {
                    _townHall = value;

                    _townHall.OnZeroHealth += OnTownHallDeath;
                }
            }



            public CachedFraction(Fraction fraction)
            {
                WarrioirCountLimit = fraction.WarrioirMaxLimit;
                Fraction = fraction.FractionType;

                _warriors = new HashSet<Warrior>();
            }



            public void CacheBuilding(Building building)
            {
                if (_townHallIsDead)
                    throw new UnityException($"Townhall fo fraction {Fraction} has been destroyed, so you can't cache buildings of fraction anymore");

                _townHall.AddBuilding(building);
            }

            public void UncacheBuilding(Building building)
            {
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
                    _isWarrioirLimitOverflow = true;
                    OnWarrioirLimitOverflow?.Invoke();
                }
            }

            private void DecreaseWarriorCount()
            {
                _warriorCount--;

                if (_isWarrioirLimitOverflow && _warriorCount < WarrioirCountLimit)
                {
                    _isWarrioirLimitOverflow = false;
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