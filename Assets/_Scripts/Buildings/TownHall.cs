using System;
using UnityEngine;

namespace InfinityGame.Buildings
{
    public class TownHall : Building
    {
        private WarrioirSpawner _warrioirSpawner;

        public static TownHall Instantiate(TownHall prefab, Vector2 position, Fraction fraction)
        {
            var townHall = Instantiate(prefab);
            townHall.transform.position = position;
            townHall._spriteRenderer.sprite = fraction.TownHallSprite;
            townHall._health = fraction.TownHallHealth;
            townHall._fractionTag = fraction.Tag;

            townHall._warrioirSpawner = new WarrioirSpawner(fraction.BarracksSpawSettings, fraction.WarrioirPickStrategy);
            townHall._warrioirSpawner.SpawnerPosition = townHall.transform.position; // Barrack will never move // TODO: Возможно стоит записать это в ивент окончания сборки сцены
            townHall.OnDie += townHall._warrioirSpawner.SpawnCanceller.Cancel; // Cancel spawning if barrack dies
            GameManager.OnGenerationEnd += townHall._warrioirSpawner.StartGeneration; // Start sapwning wave in callback action

            return townHall;
        }
    }
}
