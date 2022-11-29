using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;



/// <summary>
/// Detect entities in world, using physics and filtring entities by fraction
/// </summary>
public static class EntitiesDetector
{
    /// <summary>
    /// Finds closest entity from input colletions
    /// </summary>
    /// <param name="position"></param>
    /// <param name="entities"></param>
    /// <returns>Closest entity</returns>
    public static GameEntity TryToGetClosestEntityToPosition(Vector2 position, IEnumerable<GameEntity> entities)
    {
        GameEntity closestEntity = null;
        var minimalDiscoveredDistance = float.MaxValue;

        foreach (var entity in entities)
        {
            var distanceToCurrentTarget = Vector3.Distance(entity.transform.position, position);

            if (distanceToCurrentTarget < minimalDiscoveredDistance)
            {
                closestEntity = entity;
                minimalDiscoveredDistance = distanceToCurrentTarget;
            }
        }

        return closestEntity;
    }

    /// <summary>
    /// Finds entities in radius with without input tags, and returns the closest one 
    /// </summary>
    /// <param name="position"></param>
    /// <param name="areaRadius"></param>
    /// <param name="ignoredFractions"></param>
    /// <returns>Closest entity</returns>
    public static GameEntity TryToGetClosestEntityToPosition(Vector2 position, float areaRadius, params string[] ignoredFractions)
    {
        var entitiesAround = GetEntitiesInArea(position, areaRadius, ignoredFractions);

        GameEntity closestEntity = null;
        var minimalDiscoveredDistance = float.MaxValue;

        foreach (var enemy in entitiesAround)
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

    /// <summary>
    /// Gets all entities in radius, ignore selectled tags
    /// </summary>
    /// <param name="areaCenterPosition"></param>
    /// <param name="areaRadius"></param>
    /// <param name="ignoredFractions"></param>
    /// <returns></returns>
    public static IEnumerable<GameEntity> GetEntitiesInArea(Vector2 areaCenterPosition, float areaRadius, params string[] ignoredFractions)
    {
        foreach (var detectedCollider in GetDetectedColliders(areaCenterPosition, areaRadius))
            if (detectedCollider.TryGetComponent(out GameEntity gameEntity) && !IsEntityBelongsToIgnoredFraction(gameEntity, ignoredFractions) && !gameEntity.IsDead)
                yield return gameEntity;
    }

    private static IEnumerable<Collider2D> GetDetectedColliders(Vector2 areaCenter, float radius) => Physics2D.OverlapCircleAll(areaCenter, radius);

    private static bool IsEntityBelongsToIgnoredFraction(GameEntity gameEntity, IEnumerable<string> ignoredTags)
    {
        foreach (var allyTag in ignoredTags)
            if (gameEntity.IsBelongsToFraction(allyTag))
                return true;

        return false;
    }

}
