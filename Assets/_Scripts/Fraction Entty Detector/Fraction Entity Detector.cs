using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


/// <summary>
/// Detect all fraction entities in area , ignoring selected fractions
/// </summary>
public class FractionEntityDetector
{
    private readonly float _areaRadius;
    private readonly string[] _ignoredFractions;



    public FractionEntityDetector(float raduis, params string[] ignoreFractions)
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

    private bool IsColliderDetectableEntity(Collider2D collider, out GameEntity detectedEntity)
    {
        if(collider.TryGetComponent(out detectedEntity))
        {
            foreach (var tag in _ignoredFractions)
                if (detectedEntity.IsBelongsToFraction(tag))
                    return false;

            return true;
        }

        return false;
    }

}
