using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;

/// <summary>
/// Detect all fraction entities in area , ignoring selected fractions
/// </summary>
public class FractionEntityDetector
{
    private readonly float _areaRadius;
    private readonly IReadOnlyList<string> _ignoredFractionTags;



    public FractionEntityDetector(float raduis, params string[] fractionsToIgnore)
    {
        _areaRadius = raduis;
        _ignoredFractionTags = fractionsToIgnore;
    }



    public IEnumerable<FractionEntity> GetDetectedFractionEntities(Vector2 areaCenter)
    {
        foreach (var detectedCollider in GetDetectedColliders(areaCenter))
            if (IsColliderDetectableEntity(detectedCollider, out FractionEntity entity))
                yield return entity;
    }

    private IEnumerable<Collider2D> GetDetectedColliders(Vector2 areaCenter) => Physics2D.OverlapCircleAll(areaCenter, _areaRadius);

    private bool IsColliderDetectableEntity(Collider2D collider, out FractionEntity detectedEntity)
    {
        var isEntity = collider.TryGetComponent(out detectedEntity);

        if (!isEntity)
            return false;

        foreach (var ignoreFractionTag in _ignoredFractionTags)
            if (detectedEntity.IsBelongToFraction(ignoreFractionTag))
                return false;

        return true;
    }
}
