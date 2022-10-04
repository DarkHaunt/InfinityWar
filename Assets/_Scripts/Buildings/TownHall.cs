using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using InfinityGame.SpawnStrategies;
using System.Threading;

namespace InfinityGame.Buildings
{
    public class TownHall : Building, IBarrack
    {
        private HitableEntity _globalTarget;


        public HitableEntity GlobalTarget => _globalTarget;
        public ISpawnStrategy SpawnStrategy { get; set; }
        public CancellationTokenSource CancellationSpawnTokenSource { get; set; }
        public Vector3 Position => transform.position;


        public static TownHall Instantiate(TownHall prefab, Fraction fraction, Action onStartSpawn)
        {
            var townHall = Instantiate(prefab);
            townHall._spriteRenderer.sprite = fraction.TownHallSprite;
            townHall._health = fraction.TownHallHealth;
            townHall._fractionTag = fraction.Tag;

            var spawnData = fraction.SpawnData;
            townHall.SpawnStrategy = StaticData.StrategiesRealization[spawnData.SpawnType].Invoke(spawnData);
            townHall.CancellationSpawnTokenSource = new CancellationTokenSource();
            townHall.OnDie += townHall.CancellationSpawnTokenSource.Cancel; // Cancel spawning if barrack dies
            onStartSpawn += townHall.SpawnWave; // Start sapwning wave in callback action

            return townHall;
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
