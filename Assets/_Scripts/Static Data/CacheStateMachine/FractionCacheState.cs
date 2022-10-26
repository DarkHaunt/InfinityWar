using System.Collections;
using System.Collections.Generic;
using InfinityGame.GameEntities;
using UnityEngine;

public abstract class FractionCacheState
{
    private IEnumerable<FractionEntity> _entities;

    public FractionCacheState(IEnumerable<FractionEntity> entities)
    {
        _entities = entities;
    }

}
