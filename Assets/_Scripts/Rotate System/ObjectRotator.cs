using UnityEngine;



public static class ObjectRotator
{
    // Starting point of angle is X axis
    // That means, that all angle calculations will be based on X axis
    // To rotate object to target by Y axis, we should consider difference between X and Y axises
    private const float AngleBetweenAxises = 90f;



    /// <summary>
    /// Rotates object to target, to make an object Y axis point on target
    /// </summary>
    public static void RoteteObjectYAxisToTarget(Rigidbody2D sourceRigidBody, Vector2 direction)
    {
        sourceRigidBody.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - AngleBetweenAxises;
    }
}
