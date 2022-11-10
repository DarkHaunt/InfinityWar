using UnityEngine;
using InfinityGame.GameEntities;

namespace InfinityGame.Projectiles
{
    public class ExplosiveProjectile : Projectile
    {
        [Range(0.5f, 10f)]
        [SerializeField] private float _splashRadius = 1f;

        private FractionEntityDetector _enemyDetector;


        protected override void OnCollisionWith(GameEntity target)
        {
            var detectedEnemies = _enemyDetector.GetDetectedFractionEntities(target.transform.position);

            foreach (var enemy in detectedEnemies)
                enemy.GetDamage(Damage);

            EndExploitation();
        }



        protected override void Awake()
        {
            _enemyDetector = new FractionEntityDetector(_splashRadius, FractionTag);

            base.Awake();
        }
    }
}
