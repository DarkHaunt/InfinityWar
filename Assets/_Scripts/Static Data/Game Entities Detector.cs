using System.Collections.Generic;
using System;
using InfinityGame.GameEntities;
using UnityEngine;



/// <summary>
/// Detect all fraction entities in area , ignoring selected fractions
/// </summary>
public static class GameEntitiesDetector
{
    public static GameEntity GetClosestEntity(Vector2 position,IEnumerable<GameEntity> entities)
    {
        GameEntity closestEntity = null;
        var minimalDiscoveredDistance = float.MaxValue;

        foreach (var enemy in entities)
        {
            var distanceToCurrentTarget = Vector3.Distance(enemy.transform.position, position);

            if (distanceToCurrentTarget < minimalDiscoveredDistance)
            {
                closestEntity = enemy;
                minimalDiscoveredDistance = distanceToCurrentTarget;
            }
        }

        return closestEntity;
    }

    public static GameEntity GetClosestEntity(Vector2 position, float areaRadius, Func<GameEntity, bool> ignoreFilterPredicate, params string[] ignoredFractions)
    {
        var entitiesAround = GetEntitiesInArea(position, areaRadius, ignoredFractions);

        GameEntity closestEntity = null;
        var minimalDiscoveredDistance = float.MaxValue;

        foreach (var enemy in entitiesAround)
        {
            if (ignoreFilterPredicate.Invoke(enemy))
                continue;

            var distanceToCurrentTarget = Vector3.Distance(enemy.transform.position, position);

            if (distanceToCurrentTarget < minimalDiscoveredDistance)
            {
                closestEntity = enemy;
                minimalDiscoveredDistance = distanceToCurrentTarget;
            }
        }

        return closestEntity;
    }

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
