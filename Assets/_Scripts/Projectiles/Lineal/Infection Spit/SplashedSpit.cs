using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class SplashedSpit : Projectile
    {
        [Range(0.5f, 10f)]
        [SerializeField] private float _splashRadius = 1f;

        private FractionEntityDetector _enemyDetector;


        protected override void OnCollisionWith(FractionEntity target)
        {
            var detectedEnemies = _enemyDetector.GetDetectedFractionEntities(target.transform.position);

            foreach (var enemy in detectedEnemies)
                enemy.GetDamage(Damage);

            EndExploitation();
        }



        protected override void Awake()
        {
            InitializeDispatcher(new LinealDispatcher(Speed));
            _enemyDetector = new FractionEntityDetector(_splashRadius, FractionTag);

            base.Awake();
        }
    }
}
