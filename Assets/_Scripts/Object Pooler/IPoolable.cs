using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface IPoolable
{
    void PullInPreparations();
    void PullOutPreparation();
}
