using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace InfinityGame.Fractions.Humans
{
    public class Swordman : Warrior
    {
        [Range(0f, 1f)]
        [SerializeField] private float _nonMainTargetDamagePercent;

        [Range(0f, 3f)]
        [SerializeField] private float _atackRadius;

        private float _damageForSurroundedWarrioirs; // Damage for every non-main target warrioirs in attack radidus

        protected override void Attack()
        {
            _currentTarget.GetDamage(_damage);

            var colliders = Physics2D.OverlapCircleAll(transform.position, _atackRadius, LayerMask.GetMask(StaticData.WarrioirsLayerName));

            foreach (var collider in colliders)
                if (collider.TryGetComponent(out Warrior enemy))
                    enemy.GetDamage(_damageForSurroundedWarrioirs);
        }

        protected override bool IsOnArguingDistance()
        {
            throw new System.NotImplementedException();
        }


        protected override void Awake()
        {
            base.Awake();

            _damageForSurroundedWarrioirs = _damage * _nonMainTargetDamagePercent;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

        }
    } 
}
