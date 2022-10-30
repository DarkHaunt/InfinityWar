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
            Initialize(fraction.Tag, fractionBuildingData);

            _warriorSpawner.Initialize(fraction);

            var fractionCachedData = FractionCacher.TryToGetFractionCashedData(_fractionTag);
            fractionCachedData.OnWarrioirLimitRelease += _warriorSpawner.StartSpawning;
            fractionCachedData.OnWarrioirLimitOverflow += _warriorSpawner.StopSpawning;

            OnZeroHealth += () =>
            {
                fractionCachedData.OnWarrioirLimitRelease -= _warriorSpawner.StartSpawning;
                fractionCachedData.OnWarrioirLimitOverflow -= _warriorSpawner.StopSpawning;
            };
        }



        private void Awake()
        {
            _warriorSpawner = GetComponent<WarrioirSpawner>();
        }
    }
}
