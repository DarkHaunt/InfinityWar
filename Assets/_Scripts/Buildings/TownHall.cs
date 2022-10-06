using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.SpawnStrategies;
using System.Threading;

namespace InfinityGame.Buildings
{
    public class TownHall : Building
    {
       // private List<Warrior> _warrioisToSpawn;

        private WarrioirSpawner _warrioirSpawner;


/*        public IWarrioirChoseStrategy SpawnStrategy { get; set; }
        public CancellationTokenSource SpawnCanceller { get; set; }
        public float NextSpawnTime { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public float SpawnTimeDelta { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }*/

       // public List<Warrior> WarrioisToSpawn => _warrioisToSpawn;


        public static TownHall Instantiate(TownHall prefab, Fraction fraction, Action onStartSpawn)
        {
            var townHall = Instantiate(prefab);
            townHall._spriteRenderer.sprite = fraction.TownHallSprite;
            townHall._health = fraction.TownHallHealth;
            townHall._fractionTag = fraction.Tag;

            townHall._warrioirSpawner = new WarrioirSpawner(fraction.BarracksSpawnData, StaticData.GetStrategyByType[fraction.SpawnStrategyType]?.Invoke());
            townHall.OnDie += townHall._warrioirSpawner.SpawnCanceller.Cancel; // Cancel spawning if barrack dies
            onStartSpawn += townHall._warrioirSpawner.StartGeneration; // Start sapwning wave in callback action

            return townHall;
        }

/*        public async void GenerateWarrioirsWaves()
        {
            while (!SpawnCanceller.Token.IsCancellationRequested)
            {
                var taskOfGettingWave = SpawnStrategy.ChoseWarrioirsToSawn(WarrioisToSpawn);

              *//*  await taskOfGettingWave;

                var warriors = taskOfGettingWave.Result;*/

               /* foreach (var warrior in warriors)
                    Instantiate(warrior);*//*
            }
        }*/
    }
}
