using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.GameEntities;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.Fractions;

namespace InfinityGame.DataCaching
{
    public static class FractionCacher
    {
        public static event Action OnGameEnd;

        private static readonly Dictionary<string, FractionCachedData> _cachedFractions = new Dictionary<string, FractionCachedData>();



        public static IEnumerable<FractionEntity> GetEnemyEntitiesOfFraction(string fractionTag)
        {
            foreach (var fractionData in _cachedFractions)
            {
                if (fractionData.Key == fractionTag)
                    continue;

                foreach (var entity in fractionData.Value.GetEntities)
                    yield return entity;
            }
        }

        public static IEnumerable<FractionEntity> GetAllyEntitiesOfFraction(string fractionTag)
        {
            foreach (var entity in _cachedFractions[fractionTag].GetEntities)
                yield return entity;
        }

        public static IEnumerable<Building> GetBuildingsOfFraction(string fractionTag) => _cachedFractions[fractionTag].GetBuildings;

        public static void CashFraction(Fraction fraction)
        {
            if (_cachedFractions.ContainsKey(fraction.Tag))
                throw new UnityException($"{fraction} is already cashed, but you're trying to cash it again.");

            var fractionCashedData = new FractionCachedData(fraction);
            _cachedFractions.Add(fraction.Tag, fractionCashedData);

            fractionCashedData.OnFractionLose += () => UncacheFractionData(fractionCashedData);
        }

        private static void UncacheFractionData(FractionCachedData fractionCashedData)
        {
            if (!_cachedFractions.ContainsKey(fractionCashedData.FractionTag))
                throw new UnityException($"{fractionCashedData.FractionTag} doesn't conatin in cashe, and you're trying uncash it");

            _cachedFractions.Remove(fractionCashedData.FractionTag);

            if (_cachedFractions.Count <= 1)
                OnGameEnd?.Invoke();
        }

        public static void CacheBuilding(Building building)
        {
            _cachedFractions[building.FractionTag].CacheBuilding(building);
        }

        public static void UncacheBuilding(Building building)
        {
            _cachedFractions[building.FractionTag].UncacheBuilding(building);
        }

        public static void CacheWarrior(Warrior warrior)
        {
            _cachedFractions[warrior.FractionTag].CacheWarrior(warrior);
        }

        public static void UncacheWarrior(Warrior warrior)
        {
            _cachedFractions[warrior.FractionTag].UncacheWarrior(warrior);
        }

        public static FractionCachedData TryToGetFractionCashedData(string fractionTag)
        {
            if (_cachedFractions.TryGetValue(fractionTag, out FractionCachedData fractionData))
                return fractionData;

            throw new UnityException($"Fraction Cahser doesn't contain fraction {fractionTag}");
        }



        public class FractionCachedData
        {
            public event Action OnFractionLose;
            public event Action OnWarrioirLimitOverflow;
            public event Action OnWarrioirLimitRelease;

            private readonly int WarrioirCountLimit;

            private readonly HashSet<Building> _buildings;
            private readonly HashSet<Warrior> _warriors;

            private int _warriorCount = 0;
            private EntityDistributionState _distributionState;

            private bool _isWarrioirLimitOverflow = false;



            public IEnumerable<FractionEntity> GetEntities
            {
                get
                {
                    return _distributionState switch
                    {
                        EntityDistributionState.DistributeBuildingsOnly => _buildings,
                        EntityDistributionState.DistributeWarriorsOnly => _warriors,
                        _ => throw new UnityException($"Fraction Casher state machine can't find the state!"),
                    };
                }
            }

            public IEnumerable<Building> GetBuildings => _buildings;

            public string FractionTag { get; }



            public FractionCachedData(Fraction fraction)
            {
                WarrioirCountLimit = fraction.WarrioirMaxLimit;
                FractionTag = fraction.Tag;

                _buildings = new HashSet<Building>();
                _warriors = new HashSet<Warrior>();
                _distributionState = EntityDistributionState.DistributeBuildingsOnly;
            }



            public void CacheBuilding(Building building)
            {
                if (!_buildings.Add(building))
                    throw new UnityException($"{building} building is already cashed, but you're trying to cash it again.");

                UpdateState();
            }

            public void UncacheBuilding(Building building)
            {
                if (!_buildings.Remove(building))
                    throw new UnityException($"{building} building is not cashed, but you're trying to uncash it.");

                if (_buildings.Count == 0)
                    UpdateState();

                CheckForLose();
            }

            private void CheckForLose()
            {
                if (_buildings.Count == 0 && _warriors.Count == 0)
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
                CheckForLose();
            }

            private void IncreaseWarrioirCount()
            {
                _warriorCount++;

                if (_warriorCount >= WarrioirCountLimit)
                {
                    _isWarrioirLimitOverflow = true;
                    OnWarrioirLimitOverflow.Invoke();

/*                    if (FractionTag.Contains("Z"))
                        MonoBehaviour.print("ZOMBIE FRACTION OVERLOADED");*/
                   // WarriorFactory.StopSpawningWarrioirsOfFraction(FractionTag);
                }
            }

            private void DecreaseWarriorCount()
            {
                _warriorCount--;

                if (_isWarrioirLimitOverflow && _warriorCount < WarrioirCountLimit)
                {
                    _isWarrioirLimitOverflow = false;
                    OnWarrioirLimitRelease.Invoke();
                    //WarriorFactory.ContinueSpawningWarrioirsOfFraction(FractionTag);
                }
            }

            private void UpdateState()
            {
                _distributionState = _distributionState switch
                {
                    EntityDistributionState.DistributeBuildingsOnly => EntityDistributionState.DistributeWarriorsOnly,
                    EntityDistributionState.DistributeWarriorsOnly => EntityDistributionState.DistributeBuildingsOnly,
                    _ => throw new Exception($"FractionCashedData state machine can't find the state {_distributionState}"),
                };
            }


            /// <summary>
            /// Controlls, which entities FractionCachedData should give for query 
            /// </summary>
            private enum EntityDistributionState
            {
                DistributeBuildingsOnly,
                DistributeWarriorsOnly
            }
        }
    }
}
