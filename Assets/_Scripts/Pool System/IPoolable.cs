using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IPoolable
{
    /// <summary>
    /// Tag, which will give a object unique key in pool
    /// </summary>
    public string PoolTag { get; }

    void PullInPreparations();
    void PullOutPreparation();
}
