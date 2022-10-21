using InfinityGame.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IObjectRotateStrategy
{
    void RoteteObjectToTarget(Rigidbody2D objectRigidbody2D, Transform target);
}
