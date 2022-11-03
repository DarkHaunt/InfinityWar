using InfinityGame.Spawning;
using InfinityGame.Strategies.WarrioirPickStrategies;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(WarrioirSpawner))]
    public class Demonologist : Shooter
    {
        [SerializeField] private WarrioirSpawner.SpawnData _spawnData;
        [SerializeField] private WarrioirsPickStrategy _pickStrategy;


        private WarrioirSpawner _fractionSpawner;



        protected override void Awake()
        {
            base.Awake();

            _fractionSpawner = GetComponent<WarrioirSpawner>();
            _fractionSpawner.Initialize(Fraction, _spawnData, _pickStrategy);

            OnZeroHealth += _fractionSpawner.OnSpawnerDeactivate;
        }
    } 
}
