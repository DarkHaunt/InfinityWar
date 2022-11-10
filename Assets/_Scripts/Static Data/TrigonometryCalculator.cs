using UnityEngine;



/// <summary>
/// Gives several methods to do trigonometrical operations
/// Uses Vector type to implement world point
/// </summary>
public static class TrigonometryCalculator
{
    private static readonly Vector2 UnitCircleCenterPosition = Vector2.zero;



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
        // Normilize point for unit circle manipulator
        var normilizedPoint = pointCoordinates.normalized;

        // Quater in decart coordinate system
        var paseedQuaters = GetPassedUnitCirlceQuaters(normilizedPoint);
        var currentQuater = paseedQuaters + 1;

        // Set projection on axis to form right triangle
        Vector2 projectionEndCoordinate;

        if (currentQuater % 2 == 0) // If point is on 2nd or 4th quater - projection is made on the X axis of unit circle
            projectionEndCoordinate = new Vector2(UnitCircleCenterPosition.x, normilizedPoint.y);
        else
            projectionEndCoordinate = new Vector2(normilizedPoint.x, UnitCircleCenterPosition.y); ;


        var hypotenuse = GetVectorLength(UnitCircleCenterPosition, normilizedPoint);
        var oppositeCathetus = GetVectorLength(projectionEndCoordinate, normilizedPoint);

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
        var isDirectionPointUnderTheYAxis = UnitCircleCenterPosition.y > point.y;
        var isOnRightHalfOfCircle = UnitCircleCenterPosition.x > point.x;


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
