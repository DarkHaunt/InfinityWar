using System;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    /// <summary>
    /// Object, that belong to fraction and will ignore same fraction enteties
    /// </summary>
    public class GameEntity : MonoBehaviour
    {
        public event Action OnDie;


        [SerializeField] protected string _fractionTag;
        [SerializeField] protected float _health;

        protected bool _isDead = false; 



        public string FractionTag => _fractionTag;



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

        public bool IsBelongsToFraction(string fraction) => _fractionTag.Contains(fraction);

        public override string ToString() => $"{name} {transform.position} {FractionTag}";
    } 
}
