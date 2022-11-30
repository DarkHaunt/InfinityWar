using InfinityGame.FractionsData;
using UnityEngine;



namespace InfinityGame.GameEntities.Buildings
{
    using BuildingInitData = FractionInitData.BuildingInitData;

    /// <summary>
    /// An square-type entity, that can't move
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : GameEntity
    {
        public virtual void Initialize(FractionInitData fraction, BuildingInitData buildingData)
        {
            Init(fraction.FractionTag, buildingData.BuildingHealthPoints);

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
