using System;
using InfinityGame.Fractions;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    /// <summary>
    /// Object, that belong to fraction and will ignore same fraction enteties
    /// </summary>
    public class GameEntity : FractionHandler
    {
        public event Action OnZeroHealth;

        [SerializeField] protected float _health;

        protected bool _isDead = false; 



        public void GetDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0 && !_isDead)
            {
                _isDead = true;
                OnZeroHealth?.Invoke();
                return;
            }
        }

        public void Die()
        {
            OnZeroHealth?.Invoke();
            Destroy(gameObject);
        }

        public override string ToString() => $"{name} {transform.position} {Fraction}";
    } 
}
