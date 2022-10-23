using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IPoolable
{
    public string PoolTag { get; }

    void PullInPreparations();
    void PullOutPreparation();
}
