using InfinityGame.Spawning;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(WarrioirSpawner))]
    public class Demonologist : Shooter
    {
        [Header("--- Spawn Parameters ---")]
        [SerializeField] private WarrioirSpawner.SpawnData _spawnData;

        private WarrioirSpawner _fractionSpawner;



        protected override void Awake()
        {
            base.Awake();

            _fractionSpawner = GetComponent<WarrioirSpawner>();
            _fractionSpawner.Initialize(Fraction, _spawnData);

            OnZeroHealth += _fractionSpawner.DeactivateSpawning;
        }
    } 
}
