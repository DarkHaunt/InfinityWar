using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace InfinityGame.Buildings
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public abstract class Building : FractionEntity
    {
        protected SpriteRenderer _spriteRenderer;

        protected virtual void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
    } 
}
