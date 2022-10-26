using InfinityGame.GameEntities;
using InfinityGame.CashedData;
using InfinityGame.Fractions;
using UnityEngine;

namespace InfinityGame.Factories.BuildingFactory
{
    using FractionBuildingData = Fraction.FractionBuildingData;

    public class BuildingFactory
    {
        private Building _prefab;

        public BuildingFactory(Building buildingPrefab)
        {
            _prefab = buildingPrefab;
        }


        public Building CreateBuilding(FractionBuildingData fractionBuildingData, Vector2 position)
        {
            var building = Building.Instantiate(_prefab, fractionBuildingData.FractionTag, fractionBuildingData.BuildingData);
            building.transform.position = position;
            building.name = fractionBuildingData.Name;

            FractionCasher.CacheBuilding(building);
            //BuildingCasher.CashBuilding(building);

            return building;
        }

        public Building CreateSpawnBuilding(Fraction fraction, Vector2 position, Fraction.BuildingData buildingData) // TODO: Название не отражает действия
        {
            var fractionBarrackData = new FractionBuildingData(fraction.Tag, buildingData);
            var building = CreateBuilding(fractionBarrackData, position);

            var barrackSpawner = building.gameObject.AddComponent<WarrioirSpawner>();
            barrackSpawner.Initialize(fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

            return building;
        }
    }
}
