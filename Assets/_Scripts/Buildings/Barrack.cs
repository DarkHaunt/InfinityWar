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
    public class Barrack : Building, IBarrack
    {
        public HitableEntity GlobalTarget { get; set; }    
        public ISpawnStrategy SpawnStrategy { get; set; }
        public CancellationTokenSource CancellationSpawnTokenSource { get; set; }
        public Vector3 Position => transform.position;


        public static Barrack Instantiate(Barrack prefab, Fraction fraction, Action onStartSpawn)
        {
            var barrack = Instantiate(prefab);
            var spawnData = fraction.SpawnData;
            barrack._health = fraction.BarrackHealth;
            barrack._spriteRenderer.sprite = fraction.BarrackSprite;
            barrack._fractionTag = fraction.Tag;

            barrack.SpawnStrategy = StaticData.StrategiesRealization[spawnData.SpawnType].Invoke(spawnData);
            barrack.CancellationSpawnTokenSource = new CancellationTokenSource();
            barrack.OnDie += barrack.CancellationSpawnTokenSource.Cancel; // Cancel spawning if barrack dies
            onStartSpawn += barrack.SpawnWave; // Start sapwning wave in callback action

            return barrack;
        }

        public async void SpawnWave()
        {
            while (CancellationSpawnTokenSource.Token.IsCancellationRequested)
            {
                var taskOfGettingWave = SpawnStrategy.GetWaveWarriors(CancellationSpawnTokenSource.Token);

                await taskOfGettingWave;

                var warriors = taskOfGettingWave.Result;

                foreach (var warrior in warriors)
                {
                    // Set target for each
                }
            }
        }
    }
}
