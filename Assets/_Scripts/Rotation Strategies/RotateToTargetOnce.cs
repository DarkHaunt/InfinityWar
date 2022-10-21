using InfinityGame.Projectiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Rotates target only one time per method invoking
/// Rotates object , that it's y local axis points at target
/// </summary>
public class RotateToTargetOnce : IObjectRotateStrategy
{
    // Starting point of angle is X axis
    // That means, that all angle calculations will be based on X axis
    // To rotate object to target by Y axis, we should consider difference between X and Y axises
    private const float AngleBetweenAxises = 90f;

    public void RoteteObjectToTarget(Rigidbody2D objectRigidBody, Transform target)
    {
        var xDistance = target.position.x - objectRigidBody.transform.position.x;
        var yDistance = target.position.y - objectRigidBody.transform.position.y;

        objectRigidBody.rotation = Mathf.Atan2(yDistance, xDistance) * Mathf.Rad2Deg - AngleBetweenAxises;
    }
}
