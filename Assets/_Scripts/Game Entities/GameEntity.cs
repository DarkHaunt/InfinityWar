using System;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    /// <summary>
    /// Object, that belong to fraction and can be damaged
    /// </summary>
    public class GameEntity : MonoBehaviour
    {
        public event Action OnDie;

        [Header("--- Entity Parameters ---")]
        [SerializeField] private string _fraction;
        [SerializeField] private float _health;

        // Flags
        private bool _isDead = false;



        public string Fraction => _fraction;
        public float Health => _health;
        public bool IsDead => _isDead;



        public void Init(string fraction, float health)
        {
            _fraction = fraction;
            _health = health;

            if (_isDead)
                _isDead = false;
        }

        public void GetDamage(float damage)
        {
            if (_isDead)
                return;

            _health -= damage;

            if (_health <= 0)
            {
                _isDead = true;
                OnDie?.Invoke();
                return;
            }
        }

        public void Die()
        {
            OnDie?.Invoke();
            Destroy(gameObject);
        }

        public bool IsBelongsToFraction(string fraction) => _fraction.Contains(fraction);

        public override string ToString() => $"{name} {transform.position} {Fraction} {GetHashCode()}";
    }
}
