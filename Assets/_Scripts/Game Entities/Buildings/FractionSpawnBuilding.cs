using InfinityGame.Fractions;
using UnityEngine;
using InfinityGame.DataCaching;

namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(WarrioirSpawner))]
    public class FractionSpawnBuilding : Building
    {
        private WarrioirSpawner _warriorSpawner;

        public void Initialize(Fraction fraction, Fraction.BuildingData fractionBuildingData)
        {
            Initialize(fraction.FractionType, fractionBuildingData);

            _warriorSpawner.Initialize(fraction);

            var fractionCachedData = FractionCacher.TryToGetFractionCachedData(Fraction);
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
            _warriorSpawner = GetComponent<WarrioirSpawner>();
        }
    }
}
