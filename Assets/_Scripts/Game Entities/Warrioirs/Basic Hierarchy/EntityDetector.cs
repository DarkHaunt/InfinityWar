using System.Collections;
using System;
using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class EntityDetector : MonoBehaviour
{
    public event Action<GameEntity> OnEntityEnter;
    public event Action<GameEntity> OnEntityExit;

    private readonly HashSet<GameEntity> _detectedEntities = new HashSet<GameEntity>();



    public IEnumerable<GameEntity> DetecedEntities => _detectedEntities;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GameEntity enemy) && _detectedEntities.Add(enemy))
            OnEntityEnter?.Invoke(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out GameEntity enemy) && _detectedEntities.Remove(enemy))
            OnEntityExit?.Invoke(enemy);
    }

}
