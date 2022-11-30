using UnityEngine;
using System.Collections.Generic;
using System;



namespace InfinityGame.GameEntities.Buildings
{
    /// <summary>
    /// A class, that determines main building of fraction<br/>
    /// If TownHall gets destroyed, fraction loses all buildiings instantly
    /// </summary>
    public class TownHall : MonoBehaviour
    {
        public event Action OnDestroy;

        private readonly HashSet<Building> _buildings = new HashSet<Building>();
        private Barrack _barrack;



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

        public void SetBarrack(Barrack barrack)
        {
            if (_barrack != null)
                throw new UnityException($"Townhall {this} already have Barrack component");

            _barrack = barrack;
            _barrack.OnDie += OnDestroy;
            _barrack.OnDie += DestroyAllBuildings;
        }

        public override string ToString() => $"{name} {transform.position}";
    }
}
