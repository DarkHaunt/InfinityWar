using UnityEngine;



namespace InfinityGame.GameEntities.MeleeWarriors
{
    /// <summary>
    /// A Demon fraction warrior, that will explde after death
    /// </summary>
    public class Demon : MeleeWarrior
    {
        [Header("--- Demon Parameters ---")]
        [SerializeField] private float _explodeDamage;

        [Range(0f, 3f)]
        [SerializeField] private float _explodeRadius;



        protected override void Attack()
        {
            LocalTarget.GetDamage(MeleeDamage);
        }

        private void Explode()
        {
            var enemies = EntitiesDetector.GetEntitiesInArea(transform.position, _explodeRadius, Fraction);

            foreach (var enemy in enemies)
                enemy.GetDamage(_explodeDamage);
        }



        private void Start()
        {
            OnDie += Explode;
        }
    }
}
