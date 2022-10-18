using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.CashedData
{
    public static class GameCasher
    {
        private static HashSet<Building> _cashedBuildings = new HashSet<Building>();


        public static IEnumerable<Building> GetCashedBuildings() => _cashedBuildings;

        public static void CashBuilding(Building building)
        {
            if (!_cashedBuildings.Add(building))
                throw new UnityException($"Townhall {building.name} {building.transform.position} is already cashed, but you're trying to cash it again.");

            building.OnDie += () => UncashBuilding(building);
        }

        private static void UncashBuilding(Building building)
        {
            if (!_cashedBuildings.Remove(building))
                throw new UnityException($"Townhall {building.name} {building.transform.position} is not cashed, but you're trying to uncash it.");
        }
    }
}
