using InfinityGame.GameEntities;
using InfinityGame.DataCaching;
using InfinityGame.Fractions;
using UnityEngine;

namespace InfinityGame.Factories.BuildingFactory
{
    using BuildingData = Fraction.BuildingData;

    public class BuildingFactory
    {
        public BuildingFactory() { }


        public BuildingType SpawnFractionBuilding<BuildingType>(Fraction fraction, BuildingData fractionBuildingData, Vector2 position) where BuildingType : Building
        {
            var buildingGameObject = new GameObject(fractionBuildingData.Name);
            var building = buildingGameObject.AddComponent<BuildingType>();

            building.Initialize(fraction, fractionBuildingData);
            building.transform.position = position;

            building.OnDie += () => Object.Destroy(building.gameObject);
            building.OnDie += () => FractionCacher.UncacheBuilding(building);

            FractionCacher.CacheBuilding(building);

            return building;
        }
    }
}
