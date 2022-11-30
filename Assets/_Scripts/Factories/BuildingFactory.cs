using InfinityGame.DataCaching;
using InfinityGame.FractionsData;
using InfinityGame.GameEntities.Buildings;
using UnityEngine;



namespace InfinityGame.Factories.BuildingFactory
{
    using BuildingInitData = FractionInitData.BuildingInitData;

    public class BuildingFactory
    {
        public BuildingFactory() { }


        public BuildingType SpawnFractionBuilding<BuildingType>(FractionInitData fraction, BuildingInitData fractionBuildingData, Vector2 position) where BuildingType : Building
        {
            var buildingGameObject = new GameObject(fractionBuildingData.Name);
            var building = buildingGameObject.AddComponent<BuildingType>();

            Init(building, fraction, fractionBuildingData, position);

            return building;
        }

        public BuildingType AddAndInitFractionBuildingComponentOn<BuildingType>(GameObject gameObject, FractionInitData fraction, BuildingInitData fractionBuildingData, Vector2 position) where BuildingType : Building
        {
            var building = gameObject.AddComponent<BuildingType>();

            Init(building, fraction, fractionBuildingData, position);

            return building;
        }

        private void Init<BuildingType>(BuildingType building, FractionInitData fraction, BuildingInitData fractionBuildingData, Vector2 position) where BuildingType : Building
        {
            building.Initialize(fraction, fractionBuildingData);
            building.transform.position = position;

            building.OnDie += () => Object.Destroy(building.gameObject);
            building.OnDie += () => FractionCacher.UncacheBuilding(building);

            FractionCacher.CacheBuilding(building);
        }
    }
}
