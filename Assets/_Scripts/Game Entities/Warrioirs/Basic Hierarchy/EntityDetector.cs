using System.Collections;
using System;
using InfinityGame.GameEntities;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CircleCollider2D))]
public class EntityDetector : MonoBehaviour
{
    public event Action<FractionEntity> OnEntityEnter;
    public event Action<FractionEntity> OnEntityExit;

    private readonly HashSet<FractionEntity> _detectedEntities = new HashSet<FractionEntity>();



    public IEnumerable<FractionEntity> DetecedEntities => _detectedEntities;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FractionEntity enemy) && _detectedEntities.Add(enemy))
            OnEntityEnter?.Invoke(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FractionEntity enemy) && _detectedEntities.Remove(enemy))
            OnEntityExit?.Invoke(enemy);
    }

}
