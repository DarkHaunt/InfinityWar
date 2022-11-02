using System.Collections.Generic;
using InfinityGame.GameEntities;
using InfinityGame.Fractions;
using UnityEngine;


/// <summary>
/// Detect all fraction entities in area , ignoring selected fractions
/// </summary>
public class FractionEntityDetector
{
    private readonly float _areaRadius;
    private readonly FractionHandler.FractionType _ignoredFractions;



    public FractionEntityDetector(float raduis, FractionHandler.FractionType ignoreFractions)
    {
        _areaRadius = raduis;
        _ignoredFractions = ignoreFractions;
    }



    public IEnumerable<GameEntity> GetDetectedFractionEntities(Vector2 areaCenter)
    {
        foreach (var detectedCollider in GetDetectedColliders(areaCenter))
            if (IsColliderDetectableEntity(detectedCollider, out GameEntity entity))
                yield return entity;
    }

    private IEnumerable<Collider2D> GetDetectedColliders(Vector2 areaCenter) => Physics2D.OverlapCircleAll(areaCenter, _areaRadius);

    private bool IsColliderDetectableEntity(Collider2D collider, out GameEntity detectedEntity) => collider.TryGetComponent(out detectedEntity) && !detectedEntity.IsBelongsToFraction(_ignoredFractions);
}
