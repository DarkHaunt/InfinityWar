using InfinityGame.Spawning;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    [RequireComponent(typeof(WarrioirSpawner))]
    public class Demonologist : Shooter
    {
        [Header("--- Spawn Parameters ---")]
        [SerializeField] private WarrioirSpawner.SpawnerInitData _spawnData;

        private WarrioirSpawner _fractionSpawner;



        public override void PullOutPreparation()
        {
            base.PullOutPreparation();

            _fractionSpawner.ActivateSpawner();
        }



        protected override void Awake()
        {
            base.Awake();

            _fractionSpawner = GetComponent<WarrioirSpawner>();
            _fractionSpawner.Initialize(Fraction, _spawnData);

            OnDie += _fractionSpawner.DeactivateSpawner;
        }
    } 
}
