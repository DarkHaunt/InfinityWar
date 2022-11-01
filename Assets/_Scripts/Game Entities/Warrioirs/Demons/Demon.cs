using System.Collections;
using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;

namespace InfinityGame.GameEntities.Demons
{

    public class Demon : MeleeWarrioir
    {
        [SerializeField] private float _explodeDamage;
        [SerializeField] private float _explodeRadius;

        private FractionEntityDetector _enemyDetector;



        protected override void Attack()
        {
            LocalTarget.GetDamage(MeleeDamage);
        }

        private void Explode()
        {
            var enemies = _enemyDetector.GetDetectedFractionEntities(transform.position);

            foreach (var enemy in enemies)
                enemy.GetDamage(_explodeDamage);
        }



        protected override void Awake()
        {
            base.Awake();

            _enemyDetector = new FractionEntityDetector(_explodeRadius, FractionTag);
            OnZeroHealth += Explode;
        }
    }
}
