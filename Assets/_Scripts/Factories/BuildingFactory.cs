using InfinityGame.GameEntities;
using InfinityGame.DataCaching;
using InfinityGame.Fractions;
using UnityEngine;

namespace InfinityGame.Factories.BuildingFactory
{
    using FractionBuildingData = Fraction.FractionBuildingData;

    public class BuildingFactory
    {
        public BuildingFactory() { }



        public Building CreateBuilding(FractionBuildingData fractionBuildingData, Vector2 position)
        {
            var buildingGameObject = new GameObject(fractionBuildingData.Name);
            var building = buildingGameObject.AddComponent<Building>();

            building.Initialize(fractionBuildingData.FractionTag, fractionBuildingData.BuildingData);
            building.transform.position = position;
            building.name = fractionBuildingData.Name;

            FractionCacher.CacheBuilding(building);
            building.OnZeroHealth += () => FractionCacher.UncacheBuilding(building);

            return building;
        }

        public Building CreateAndInitializeSpawnBuilding(Fraction fraction, Vector2 position, Fraction.BuildingData buildingData)
        {
            var fractionBarrackData = new FractionBuildingData(fraction.Tag, buildingData);
            var building = CreateBuilding(fractionBarrackData, position);

            var barrackSpawner = building.gameObject.AddComponent<WarrioirSpawner>();
            barrackSpawner.Initialize(fraction.Tag ,fraction.WarrioirSpawnSettings, fraction.WarrioirPickStrategy);

            return building;
        }
    }
}
