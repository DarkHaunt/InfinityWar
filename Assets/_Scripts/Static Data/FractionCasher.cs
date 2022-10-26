using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.GameEntities;
using InfinityGame.Factories.WarriorFactory;
using InfinityGame.Fractions;

namespace InfinityGame.CashedData
{
    using FractionCashedData = FractionCasher.FractionCashedData;

    public static class FractionCasher
    {
        public static event Action OnGameEnd;


        private static readonly HashSet<FractionCashedData> _fractionsData = new HashSet<FractionCashedData>();


        public static IEnumerable<FractionEntity> GetEnemyEntities(string fractionTag)
        {
            foreach (var fractionData in _fractionsData)
            {
                if (fractionData.FractionTag == fractionTag)
                    continue;

                foreach (var entity in fractionData.GetEntities)
                    yield return entity;
            }
        }

        public static IEnumerable<FractionEntity> GetAllyEntities(string fractionTag)
        {
            foreach (var fractionData in _fractionsData)
            {
                if (fractionData.FractionTag != fractionTag)
                    continue;

                foreach (var entity in fractionData.GetEntities)
                    yield return entity;

                break;
            }
        }

        public static void CashFraction(Fraction fraction)
        {
            var fractionCashedData = new FractionCashedData(fraction);

            if (!_fractionsData.Add(fractionCashedData))
                throw new UnityException($"{fraction} is already cashed, but you're trying to cash it again.");

            fractionCashedData.OnFractionLose += () => RemoveFractionFromCashe(fractionCashedData);
        }

        private static void RemoveFractionFromCashe(FractionCashedData fractionCashedData)
        {
            if (!_fractionsData.Remove(fractionCashedData))
                throw new UnityException($"{fractionCashedData.FractionTag} doesn't conatin in cashe, and you're trying uncash it");

            if (_fractionsData.Count <= 1)
                OnGameEnd?.Invoke();
        }

        public static void CacheBuilding(Building building)
        {
            foreach (var fractionData in _fractionsData)
                if (building.IsSameFraction(fractionData.FractionTag))
                {
                    fractionData.CacheBuilding(building);
                    return;
                }


            throw new UnityException($"Building {building} can't be cashed. Fraction can't be found");
        }

        public static void CacheWarrior(Warrior warrioir)
        {
            foreach (var fractionData in _fractionsData)
                if (warrioir.IsSameFraction(fractionData.FractionTag))
                {
                    fractionData.CacheWarrior(warrioir);
                    return;
                }


            throw new UnityException($"Warrioir {warrioir} can't be cashed. Fraction can't be found");
        }




        public class FractionCashedData
        {
            public event Action<string> OnMaxFractionWarrioirs;
            public event Action OnFractionLose;

            private readonly int WarrioirMaxCount;

            private HashSet<Building> _fractionBuildings;
            private HashSet<Warrior> _fractionWarriors;

            private int _warriorCount = 0;
            private OutputState _cacheState;



            public IEnumerable<FractionEntity> GetEntities
            {
                get
                {
                    return _cacheState switch
                    {
                        OutputState.Buildings => _fractionBuildings,
                        OutputState.Warriors => _fractionWarriors,
                        _ => throw new UnityException($"Fraction Casher state machine can't find the state!"),
                    };
                }
            }

            public string FractionTag { get; }



            public FractionCashedData(Fraction fraction)
            {
                WarrioirMaxCount = fraction.WarrioirMaxCount;
                FractionTag = fraction.Tag;

                _fractionBuildings = new HashSet<Building>();
                _fractionWarriors = new HashSet<Warrior>();
                _cacheState = OutputState.Buildings; // TODO: State machine с классами и ссылками на коллекции войнов \ строений
            }



            public void CacheBuilding(Building building)
            {
                if (!_fractionBuildings.Add(building))
                    throw new UnityException($"{building} building is already cashed, but you're trying to cash it again.");

                building.OnZeroHealth += () => UncacheBuilding(building);

                if (_cacheState == OutputState.Warriors)
                    _cacheState = OutputState.Buildings;
            }

            private void UncacheBuilding(Building building)
            {
                if (!_fractionBuildings.Remove(building))
                    throw new UnityException($"{building} building is not cashed, but you're trying to uncash it.");

                if (_fractionBuildings.Count == 0)
                    _cacheState = OutputState.Warriors;

                CheckForLose();
            }

            private void CheckForLose()
            {
                if (_fractionBuildings.Count == 0 && _fractionWarriors.Count == 0)
                    OnFractionLose?.Invoke();
            }

            public void CacheWarrior(Warrior warrior)
            {
                if (!_fractionWarriors.Add(warrior))
                    throw new UnityException($"{warrior} warrior is already cashed, but you're trying to cash it again.");

                warrior.OnZeroHealth += () => UncacheWarrior(warrior);
                IncreaseWarrioirCount();
            }

            private void UncacheWarrior(Warrior warrior)
            {
                if (!_fractionWarriors.Remove(warrior))
                    throw new UnityException($"{warrior} warrior is not cashed, but you're trying to uncash it.");

                warrior.OnZeroHealth -= () => UncacheWarrior(warrior);
                CheckForLose();
            }

            private void IncreaseWarrioirCount()
            {
                _warriorCount++;

                if (_warriorCount >= WarrioirMaxCount)
                    OnMaxFractionWarrioirs?.Invoke(FractionTag);
            }



            private enum OutputState
            {
                Buildings,
                Warriors
            }
        }
    }
}
