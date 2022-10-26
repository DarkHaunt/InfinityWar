using System;
using System.Collections.Generic;
using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.CashedData
{
   /* public static class BuildingCasher
    {
        private static HashSet<Building> _cashedBuildings = new HashSet<Building>();


        public static IEnumerable<Building> GetCashedBuildings() => _cashedBuildings;

        public static bool IsOnlyOneFractionBuildingsLeft()
        {
            string decetedTag = string.Empty;

            foreach (var building in GetCashedBuildings())
            {
                if (decetedTag == string.Empty)
                {
                    decetedTag = building.FractionTag;
                    continue;
                }

                if (decetedTag != building.FractionTag)
                {
                    MonoBehaviour.print(false);
                    return false;
                }
            }

            return true;
        }

        public static void CashBuilding(Building building)
        {
            if (!_cashedBuildings.Add(building))
                throw new UnityException($"{building} is already cashed, but you're trying to cash it again.");

            building.OnZeroHealth += () =>
            {
                UncashBuilding(building);
            };
        }

        private static void UncashBuilding(Building building)
        {
            if (!_cashedBuildings.Remove(building))
                throw new UnityException($"{building} is not cashed, but you're trying to uncash it.");
        }
    }*/
}
