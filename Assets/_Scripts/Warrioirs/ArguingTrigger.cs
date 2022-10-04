using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ArguingTrigger : MonoBehaviour
{
    private const int HypotheticalNumberOfEnemiesNearby = 10; // For list optimization

    public event Action<HitableEntity> OnTargetEnter;
    public event Action<HitableEntity> OnTargetExit;

    public event Action OnTriggerDisable;

    private string _fractionTag;

    private List<HitableEntity> _hitableEntitiesInArea = new List<HitableEntity>(HypotheticalNumberOfEnemiesNearby);


    // Test
    public void YaZalupa() => print("Test call - " + name);

    public HitableEntity GetNearliestTarget()
    {
        if (_hitableEntitiesInArea.Count == 0)
            return null;

        HitableEntity target = _hitableEntitiesInArea[0];
        var minimalDiscoveredDistance = float.MaxValue;

        for (int i = 1; i < _hitableEntitiesInArea.Count; i++)
        {
            var distanceToCurrentTarget = Vector3.Distance(_hitableEntitiesInArea[i].transform.position, target.transform.position);

            if (distanceToCurrentTarget < minimalDiscoveredDistance)
            {
                target = _hitableEntitiesInArea[i];
                minimalDiscoveredDistance = distanceToCurrentTarget;
            }
        }

        return target;
    }


    private void Awake()
    {
        OnTargetEnter += (HitableEntity hitableEntity) => _hitableEntitiesInArea.Add(hitableEntity);
        OnTargetExit += (HitableEntity hitableEntity) => _hitableEntitiesInArea.Remove(hitableEntity);

        OnTriggerDisable += () => _hitableEntitiesInArea.Clear(); // Because it can't be able to track them anymore
    }

    private void OnDisable()
    {
        OnTriggerDisable?.Invoke();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitableEntity enemy) && enemy.FractionTag != _fractionTag)
            OnTargetEnter?.Invoke(enemy);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out HitableEntity enemy) && _hitableEntitiesInArea.Contains(enemy))
            OnTargetExit?.Invoke(enemy);
    }

}
