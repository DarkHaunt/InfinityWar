using System.Collections;
using System.Linq;
using System.Collections.Generic;
using InfinityGame.SpawnStrategies;
using System;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;


namespace InfinityGame.Buildings
{
    public class Barrack : Building
    {
        private WarrioirSpawner _warrioirSpawner;

        public static Barrack Instantiate(Barrack prefab, Fraction fraction, Action onStartSpawn)
        {
            var barrack = Instantiate(prefab);
            barrack._health = fraction.BarrackHealth;
            barrack._spriteRenderer.sprite = fraction.BarrackSprite;
            barrack._fractionTag = fraction.Tag;

            //barrack._warrioirSpawner = new WarrioirSpawner(fraction.BarracksSpawnData, StaticData.StrategiesRealization[fraction.SpawnType].Invoke());

            //barrack.SpawnStrategy = StaticData.StrategiesRealization[fraction.SpawnType].Invoke();
            barrack._warrioirSpawner = new WarrioirSpawner(fraction.BarracksSpawnData, StaticData.GetStrategyByType[fraction.SpawnStrategyType]?.Invoke());
            barrack.OnDie += barrack._warrioirSpawner.SpawnCanceller.Cancel; // Cancel spawning if barrack dies
            onStartSpawn += barrack._warrioirSpawner.StartGeneration; // Start sapwning wave in callback action

            return barrack;
        }

/*        public async void GenerateWarrioirsWaves()
        {
            while (!SpawnCanceller.Token.IsCancellationRequested)
            {
                var taskOfGettingWave = new Task<IEnumerable<Warrior>>(() => SpawnStrategy.ChoseWarrioirsToSawn(WarrioisToSpawn), SpawnCanceller.Token);


                // TODO: Make a random delta of time
               // var coolDown = Task.Delay();


                await taskOfGettingWave;

                var warriors = taskOfGettingWave.Result;

                foreach (var warrior in warriors)
                    Instantiate(warrior);
            }
        }*/
    }
}
