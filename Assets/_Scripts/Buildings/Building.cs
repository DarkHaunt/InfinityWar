using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.Buildings
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Building : MonoBehaviour, IHitable
    {
        public event Action OnDie;

        [SerializeField] protected float _health = 1000;
        [SerializeField] protected SpriteRenderer _spriteRenderer;


        public void GetDamage(float damage)
        {
            _health -= damage;

            if (_health <= 0)
                OnDie.Invoke();
        }


        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    } 
}
