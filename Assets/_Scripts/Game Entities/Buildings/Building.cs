using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace InfinityGame.GameEntities
{
    using BuildingData = Fractions.Fraction.BuildingData;

    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Building : FractionEntity
    {
        public static Building Instantiate(Building prefab, string fractionTag, BuildingData buildingData)
        {
            var building = MonoBehaviour.Instantiate(prefab);
            building._fractionTag = fractionTag;
            building._health = buildingData.BuildingHealthPoints;

            var buildingSpriteRenderer = building.gameObject.GetComponent<SpriteRenderer>();
            buildingSpriteRenderer.sprite = buildingData.BuildingSprite;

            var boxCollider2d = building.GetComponent<BoxCollider2D>();
            boxCollider2d.size = buildingSpriteRenderer.sprite.bounds.size;

            return building;
        }

        public override string ToString() => $"{name} {transform.position}";
    }
}
