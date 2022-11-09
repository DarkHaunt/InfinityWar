using UnityEngine;



/// <summary>
/// Rotates object to target, to make an object Y axis point on target
/// </summary>
public class ObjectRotator
{
    // Starting point of angle is X axis
    // That means, that all angle calculations will be based on X axis
    // To rotate object to target by Y axis, we should consider difference between X and Y axises
    private const float AngleBetweenAxises = 90f;

    public void RoteteObjectToTarget(Rigidbody2D objectRigidBody, Vector2 direction)
    {
        objectRigidBody.rotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - AngleBetweenAxises;
    }
}
