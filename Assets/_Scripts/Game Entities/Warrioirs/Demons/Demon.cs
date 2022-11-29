using UnityEngine;



namespace InfinityGame.GameEntities.Demons
{
    public class Demon : MeleeWarrior
    {
        [Header("--- Demon Parameters ---")]
        [SerializeField] private float _explodeDamage;
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
