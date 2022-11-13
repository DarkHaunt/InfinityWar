using UnityEngine;



namespace InfinityGame.GameEntities.Demons
{
    public class Demon : MeleeWarrior
    {
        [SerializeField] private float _explodeDamage;
        [SerializeField] private float _explodeRadius;



        protected override void Attack()
        {
            LocalTarget.GetDamage(MeleeDamage);
        }

        private void Explode()
        {
            var enemies = GameEntitiesDetector.GetEntitiesInArea(transform.position, _explodeRadius, FractionTag);

            foreach (var enemy in enemies)
                enemy.GetDamage(_explodeDamage);
        }



        protected override void Awake()
        {
            base.Awake();

            OnDie += Explode;
        }
    }
}
