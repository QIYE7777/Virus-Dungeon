using UnityEngine;

public class MathGame
{
    public static Vector3 FindNearestPointOnLine(Vector3 origin, Vector3 end, Vector3 point)
    {
        //Get heading
        Vector3 heading = (end - origin);
        float magnitudeMax = heading.magnitude;
        heading.Normalize();

        //Do projection from the point but clamp it
        Vector3 lhs = point - origin;
        float dotP = Vector3.Dot(lhs, heading);
        dotP = Mathf.Clamp(dotP, 0f, magnitudeMax);
        return origin + heading * dotP;
    }

    public static float NearestDistanceFromLine(Vector3 origin, Vector3 end, Vector3 point)
    {
        var nearestPoint = FindNearestPointOnLine(origin, end, point);
        return (nearestPoint - point).magnitude;
    }
}