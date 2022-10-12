using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArguingTrigger : MonoBehaviour
{
    public event Action<FractionEntity> OnEntityEnter;
    public event Action<FractionEntity> OnEntityExit;

    private HashSet<FractionEntity> _entitiesInArea = new HashSet<FractionEntity>();

    public HashSet<FractionEntity> GetEntitiesInTriggerArea => _entitiesInArea;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FractionEntity enemy) && GetEntitiesInTriggerArea.Add(enemy))
            OnEntityEnter?.Invoke(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out FractionEntity enemy) && GetEntitiesInTriggerArea.Remove(enemy))
            OnEntityExit?.Invoke(enemy);
    }

}
