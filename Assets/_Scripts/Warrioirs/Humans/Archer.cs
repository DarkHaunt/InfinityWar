using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.Fractions.Humans
{
    public class Archer : Warrior, IShoot
    {
        [SerializeField] private Projectile _bullerPrefab;


        public void Shoot(Vector3 targetPosition)
        {
            var direction = (targetPosition - transform.position).normalized;
            var bullet = Instantiate(_bullerPrefab);

            bullet.Throw(direction);
        }

        protected override void Attack()
        {
            var shootDirection = (_currentTarget.transform.position - transform.position).normalized;

            Shoot(shootDirection);
        }        


        protected override bool IsOnArguingDistance()
        {
            throw new System.NotImplementedException();
        }
    }
}
