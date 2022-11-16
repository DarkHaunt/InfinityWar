using InfinityGame.Fractions;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    using BuildingInitData = FractionInitData.BuildingInitData;

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : GameEntity
    {
        public virtual void Initialize(FractionInitData fraction, BuildingInitData buildingData)
        {
            _health = buildingData.BuildingHealthPoints;
            _fraction = fraction.FractionTag;

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
