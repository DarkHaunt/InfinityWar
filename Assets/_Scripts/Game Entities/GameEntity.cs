using System;
using InfinityGame.Fractions;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    /// <summary>
    /// Object, that belong to fraction and will ignore same fraction enteties
    /// </summary>
    public class GameEntity : MonoBehaviour
    {
        public event Action OnZeroHealth;


        [SerializeField] protected string _fractionTag;
        [SerializeField] protected float _health;

        protected bool _isDead = false; 



        public string Fraction => _fractionTag;



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

        public bool IsBelongsToFraction(string fraction) => _fractionTag.Contains(fraction);

        public override string ToString() => $"{name} {transform.position} {Fraction}";
    } 
}
