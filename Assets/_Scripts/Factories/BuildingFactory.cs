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

            building.Initialize(fractionBuildingData.Fraction, fractionBuildingData.BuildingData);
            building.transform.position = position;

            FractionCacher.CacheBuilding(building);

            return building;
        }

        public FractionSpawnBuilding CreateSpawnBuilding(Fraction fraction, Vector2 position, Fraction.BuildingData buildingData)
        {
            var buildingGameObject = new GameObject(buildingData.Name);
            var spawnBuilding = buildingGameObject.AddComponent<FractionSpawnBuilding>();

            spawnBuilding.Initialize(fraction, buildingData);
            spawnBuilding.transform.position = position;

            FractionCacher.CacheBuilding(spawnBuilding);

            spawnBuilding.OnZeroHealth += () => FractionCacher.UncacheBuilding(spawnBuilding);
            spawnBuilding.OnZeroHealth += () => Object.Destroy(spawnBuilding.gameObject);

            return spawnBuilding;
        }

        public TownHall CreateTownHall(Fraction fraction, Vector2 position, Fraction.BuildingData buildingData)
        {
            var buildingGameObject = new GameObject(buildingData.Name);
            var townHall = buildingGameObject.AddComponent<TownHall>();

            FractionCacher.CashFraction(fraction, townHall);

            townHall.Initialize(fraction, buildingData);
            townHall.transform.position = position;

            townHall.OnZeroHealth += () => Object.Destroy(townHall.gameObject);

            return townHall;
        }
    }
}
