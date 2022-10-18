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
        public event Action OnDie;
        public event Action OnHit;

        [SerializeField] protected float _health;
        [SerializeField] protected string _fractionTag;


        public string FractionTag => _fractionTag;


        public bool IsSameFraction(string fractionTag) => _fractionTag == fractionTag;

        public void GetDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
            {
                //OnDie?.Invoke();
                Destroy(gameObject);
                return;
            }

            OnHit?.Invoke();
        }

        public void Die()
        {
            OnDie?.Invoke();
            Destroy(gameObject);
        }
    } 
}
