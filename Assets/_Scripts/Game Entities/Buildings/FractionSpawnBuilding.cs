using InfinityGame.DataCaching;
using InfinityGame.Fractions;
using UnityEngine;

namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(WarrioirSpawner))]
    public class FractionSpawnBuilding : Building
    {
        private FractionSpawner _warriorSpawner;

        public void Initialize(Fraction fraction, Fraction.BuildingData fractionBuildingData)
        {
            Initialize(fraction.FractionType, fractionBuildingData);

            _warriorSpawner.Initialize(fraction);

            // TODO: ”брать бы от сюда это
            var fractionCachedData = FractionCacher.GetFractionCachedData(Fraction);
            fractionCachedData.OnWarrioirLimitRelease += _warriorSpawner.StartSpawning;
            fractionCachedData.OnWarrioirLimitOverflow += _warriorSpawner.StopSpawning;

            OnZeroHealth += () =>
            {
                fractionCachedData.OnWarrioirLimitRelease -= _warriorSpawner.StartSpawning;
                fractionCachedData.OnWarrioirLimitOverflow -= _warriorSpawner.StopSpawning;
            };
        }



        protected virtual void Awake()
        {
            _warriorSpawner = GetComponent<FractionSpawner>();
        }
    }
}
