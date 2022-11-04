using UnityEngine;
using System.Collections.Generic;
using System;

namespace InfinityGame.GameEntities
{
    public class TownHall : Barrack // TODO: Может вместо наследования попробовать композицию?
    {
        private readonly HashSet<Building> _buildings = new HashSet<Building>();



        public IEnumerable<Building> Buildings => _buildings;



        public void AddBuilding(Building building)
        {
            if (!_buildings.Add(building))
                throw new UnityException($"Buildings {building} is already in {this} building collection");
        }

        public void RemoveBuilding(Building building)
        {
            if (!_buildings.Remove(building))
                throw new UnityException($"Building {building} is not in {this} bulding collection, but you're trying to uncash it.");
        }

        private void DestroyAllBuildings()
        {
            Action onAllBuildingsIterationEnd = null;

            foreach (var aliveFractionBuilding in _buildings)
                    onAllBuildingsIterationEnd += aliveFractionBuilding.Die;

            onAllBuildingsIterationEnd?.Invoke();
        }



        private void Start()
        {
            OnZeroHealth += DestroyAllBuildings;
        }
    } 
}
