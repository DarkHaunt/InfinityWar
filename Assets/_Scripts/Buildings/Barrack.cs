using System.Collections;
using System.Collections.Generic;
using InfinityGame.SpawnStrategies;
using System;
using UnityEngine;


namespace InfinityGame.Buildings
{
    using SpawnType = ISpawnStrategy.SpawnType;
    using SpawnData = ISpawnStrategy.SpawnCycleData;
    public class Barrack : Building
    {
        /// <summary>
        /// Types of spawn strategies and their creation
        /// </summary>
        private static Dictionary<SpawnType, Func<SpawnData, ISpawnStrategy>> _strategiesRealization = new Dictionary<SpawnType, Func<SpawnData, ISpawnStrategy>>()
        {
            [SpawnType.Group] = new Func<SpawnData, ISpawnStrategy>((SpawnData spawnData) =>
            {
                return new GroupSpawnStrategy(spawnData);
            }),

            [SpawnType.Single] = new Func<SpawnData, ISpawnStrategy>((SpawnData spawnData) =>
            {
                return new SingleSpawnStrategy(spawnData);
            }),
        };

        private ISpawnStrategy _spawnStrategy;
        private Coroutine _savedSpawnCoroutine;


        public ISpawnStrategy SetSpawnStrategy
        {
            set
            {
                _spawnStrategy = value;
            }
        }


        public static Barrack Instantiate(Barrack prefab, Fraction fraction)
        {
            var barrack = Instantiate(prefab);
            var spawnData = fraction.SpawnData;
            barrack._health = fraction.BarrackHealth;
            barrack.SetSpawnStrategy = _strategiesRealization[spawnData.SpawnType].Invoke(spawnData);
            barrack._spriteRenderer.sprite = fraction.BarrackSprite;
            //barrack._savedSpawnCoroutine = barrack.StartCoroutine(barrack._spawnStrategy.SpawnCoroutine(fraction.FractionWarriours));

            return barrack;
        }


        protected override void Awake()
        {
            base.Awake();

            OnDie += () => StopCoroutine(_savedSpawnCoroutine);
        }


    }
}
