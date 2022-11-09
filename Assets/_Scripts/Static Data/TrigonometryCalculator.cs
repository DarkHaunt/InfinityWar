using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gives several methods to do trigonometrical operations
/// Uses Vector type to implement world point
/// </summary>
public static class TrigonometryCalculator
{
    /// <summary>
    /// Returns vector scalar length fo vector
    /// </summary>
    /// <param name="startPoint">Coordinates of vector start</param>
    /// <param name="endPoint">Coordinates of vector end</param>
    /// <returns></returns>
    public static float GetVectorLength(Vector2 startPoint, Vector2 endPoint) => Mathf.Sqrt(((endPoint.x - startPoint.x) * (endPoint.x - startPoint.x)) + ((endPoint.y - startPoint.y) * (endPoint.y - startPoint.y)));

    /// <summary>
    /// Returns angle, which needs to turn object local Y axis point on direction coordinates
    /// </summary>
    /// Uses a unit circle for calculations
    /// <param name="pointCoordinates">Point on unit circle (Magnitude musn't be more than 1)</param>
    /// <returns>Angle IN RADIANS</returns>
    public static float GetRotationAngleToPoint(Vector2 pointCoordinates)
    {
        if (pointCoordinates.magnitude > 1)
            throw new UnityException($"{pointCoordinates} vector have magnitude more than 1, so it's can't be used, as direction");

        // Quater in decart coordinate system
        var paseedQuaters = GetPassedUnitCirlceQuaters(pointCoordinates);
        var currentQuater = paseedQuaters + 1;

        // Set projection on axis to form right triangle
        var projectionEndCoordinate = ((currentQuater == 2) || (currentQuater == 4)) ? 
            new Vector2(0, pointCoordinates.y) : // If point is on 2nd or 4th quater - projection is made on the X axis of unit circle
            new Vector2(pointCoordinates.x, 0); // If point is on 1st or 3rd quater - projection is made on the Y axis of unit circle

        var unitCircleCenterCoordinates = Vector2.zero;

        var hypotenuse = GetVectorLength(unitCircleCenterCoordinates, pointCoordinates);
        var oppositeCathetus = GetVectorLength(projectionEndCoordinate, pointCoordinates);

        var currentAngle = Mathf.Asin(oppositeCathetus / hypotenuse) * Mathf.Rad2Deg;


        return ((90f * paseedQuaters) + currentAngle) * Mathf.Deg2Rad;
    }

    /// <summary>
    /// Returns count of passed decart coordinates quarters by counting counterclockwise
    /// </summary>
    /// Current quater, where point exist always will be result of this method + 1
    /// <param name="point">Coordinates of point</param>
    /// <returns>Number of passed quaters from 0 to 3</returns>
    private static int GetPassedUnitCirlceQuaters(Vector2 point)
    {
        var passedQuaters = 0; // Quaters on the unit circle
        var isDirectionPointUnderTheYAxis = 0 > point.y;
        var isOnRightHalfOfCircle = 0 > point.x;


        if (isDirectionPointUnderTheYAxis)
        {
            passedQuaters += 2;

            // Check for 3rd quater passing
            if (!isOnRightHalfOfCircle)
                passedQuaters++;
        }
        else
        {
            // Check for 1st quater passing
            if (isOnRightHalfOfCircle)
                passedQuaters++;
        }

        return passedQuaters;
    }
}
