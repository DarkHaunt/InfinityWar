using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;


/// <summary>
/// Detect all fraction entities in area , ignoring selected fractions
/// </summary>
public static class GameEntitiesDetector
{
    public static IEnumerable<GameEntity> GetEntitiesInArea(Vector2 areaCenterPosition, float areaRadius, params string[] ignoredFractions)
    {
        foreach (var detectedCollider in GetDetectedColliders(areaCenterPosition, areaRadius))
            if (detectedCollider.TryGetComponent(out GameEntity gameEntity) && IsEntityBelongsToEnemyFraction(gameEntity, ignoredFractions))
                yield return gameEntity;
    }

    private static IEnumerable<Collider2D> GetDetectedColliders(Vector2 areaCenter, float radius) => Physics2D.OverlapCircleAll(areaCenter, radius);

    private static bool IsEntityBelongsToEnemyFraction(GameEntity gameEntity, IEnumerable<string> allyTags)
    {
        foreach (var allyTag in allyTags)
            if (!gameEntity.IsBelongsToFraction(allyTag))
                return true;

        return false;
    }

}
