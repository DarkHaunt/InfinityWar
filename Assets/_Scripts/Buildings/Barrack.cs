using System;
using UnityEngine;


namespace InfinityGame.Buildings
{
    public class Barrack : Building
    {
        private WarrioirSpawner _warrioirSpawner;

        public static Barrack Instantiate(Barrack prefab,Vector2 position , Fraction fraction)
        {
            var barrack = Instantiate(prefab);
            barrack.transform.position = position;
            barrack._health = fraction.BarrackHealth;
            barrack._spriteRenderer.sprite = fraction.BarrackSprite;
            barrack._fractionTag = fraction.Tag;

            barrack._warrioirSpawner = new WarrioirSpawner(fraction.BarracksSpawSettings, fraction.WarrioirPickStrategy);
            barrack._warrioirSpawner.SpawnerPosition = barrack.transform.position; // Barrack will never move
            barrack.OnDie += barrack._warrioirSpawner.SpawnCanceller.Cancel; // Cancel spawning if barrack dies
            GameManager.OnGenerationEnd += barrack._warrioirSpawner.StartGeneration; // Start sapwning wave in callback action

            return barrack;
        }
    }
}
