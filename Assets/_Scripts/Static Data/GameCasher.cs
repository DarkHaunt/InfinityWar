using System.Collections;
using System.Collections.Generic;
using UnityEngine ;
using InfinityGame.Buildings;

namespace InfinityGame.CashedData
{
    public static class GameCasher
    {
        private static HashSet<Building> _cashedBuildings = new HashSet<Building>();


        public static IEnumerable<Building> GetCashedBuildings() => _cashedBuildings;

        public static void CashBuilding(Building townHall)
        {
            if (!_cashedBuildings.Add(townHall))
                throw new UnityException($"Townhall {townHall.name} {townHall.transform.position} is already cashed, but you're trying to cash it again.");
        }

        public static void UncashBuilding(Building townHall)
        {
            if (!_cashedBuildings.Remove(townHall))
                throw new UnityException($"Townhall {townHall.name} {townHall.transform.position} is not cashed, but you're trying to uncash it.");
        }
    }
}
