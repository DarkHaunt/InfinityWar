using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    /// <summary>
    /// Object, that belong to fraction and will ignore same fraction enteties
    /// </summary>
    public class FractionEntity : MonoBehaviour, IFractionTagable
    {
        public event Action OnZeroHealth;

        [SerializeField] protected float _health;
        [SerializeField] protected string _fractionTag;

        protected bool _isDead = false; 



        public string FractionTag => _fractionTag;



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

        public bool IsSameFraction(string fractionTag) => _fractionTag == fractionTag;

        public override string ToString() => $"{name} {transform.position} {FractionTag}";
    } 
}
