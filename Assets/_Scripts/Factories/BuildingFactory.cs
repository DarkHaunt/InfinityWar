using System.Collections;
using System.Collections.Generic;
using InfinityGame.GameEntities;
using InfinityGame.CashedData;
using UnityEngine;

namespace InfinityGame.Factories.BuildingFactory
{
    using FractionBuildingData = Fractions.Fraction.FractionBuildingData;

    public class BuildingFactory
    {
        private Building _prefab;

        public BuildingFactory(Building buildingPrefab)
        {
            _prefab = buildingPrefab;
        }


        public Building SpawnBuilding(FractionBuildingData fractionBuildingData, Vector2 position)
        {
            var building = Building.Instantiate(_prefab, fractionBuildingData.FractionTag, fractionBuildingData.BuildingData);
            building.transform.position = position;
            building.name = fractionBuildingData.Name;

            GameCasher.CashBuilding(building);

            return building;
        }
    } 
}
