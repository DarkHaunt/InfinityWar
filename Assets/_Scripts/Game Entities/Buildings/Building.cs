using System;
using System.Collections;
using System.Collections.Generic;
using InfinityGame.Fractions;
using UnityEngine;


namespace InfinityGame.GameEntities
{
    using BuildingData = Fraction.BuildingData;

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : GameEntity
    {
        public virtual void Initialize(Fraction fraction, BuildingData buildingData)
        {
            _health = buildingData.BuildingHealthPoints;
            _fractionTag = fraction.FractionTag;

            var spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = buildingData.BuildingSprite;

            var boxCollider2d = GetComponent<BoxCollider2D>();
            boxCollider2d.size = spriteRenderer.sprite.bounds.size;

            var rigidBody = GetComponent<Rigidbody2D>();
            rigidBody.gravityScale = 0f;
            rigidBody.angularDrag = 0f;
            rigidBody.drag = 0f;
            rigidBody.freezeRotation = true;
        }

        public override string ToString() => $"{name} {transform.position}";
    }
}
