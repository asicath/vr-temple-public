using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Move
{

    public static float convertAngleToUnity(float angle)
    {
        var a = angle - Mathf.PI * 0.5f;
        if (a < 0) a += Mathf.PI * 2;
        return Mathf.PI * 2 - a;
    }

    public static float convertAngleFromUnity(float angle)
    {
        var a = Mathf.PI * 2 - angle + Mathf.PI * 0.5f;
        if (a > Mathf.PI * 2) a -= Mathf.PI * 2;
        return a;
    }

    /// <summary>
    /// Gets angle in radians
    /// </summary>
    public static float getAngle(GameObject centerMark, GameObject o)
    {
        return getAngle(centerMark.transform.position, o.transform.position);
    }

    public static float getAngle(Vector3 center, Vector3 position)
    {
        var a = Mathf.Atan2(position.z - center.z, position.x - center.x);

        // Lets not deal with negative angles just to make this simple
        if (a < 0) a += Mathf.PI * 2;

        return a;
    }


    public static float getDistance(GameObject a, GameObject b)
    {
        return getDistance(a.transform.position, b.transform.position);
    }

    public static float getDistance(Vector3 a, Vector3 b)
    {
        return (a - b).magnitude;
    }


    public static Vector3 getClosestPointOnCircle(GameObject center, GameObject radiusMark, GameObject o)
    {
        var radius = (center.transform.position - radiusMark.transform.position).magnitude;
        return getClosestPointOnCircle(center.transform.position, radius, o.transform.position);
    }

    public static Vector3 getClosestPointOnCircle(Vector3 center, float radius, Vector3 p)
    {
        var angle = getAngle(center, p);
        return new Vector3(Mathf.Cos(angle) * radius + center.x, 0, Mathf.Sin(angle) * radius + center.z);
    }


    // gets the clockwise tangent point from position o
    public static Vector3 getTangentPoint(GameObject center, GameObject radiusMark, GameObject o)
    {
        var radius = (center.transform.position - radiusMark.transform.position).magnitude;
        var angle = getAngle(center, o);
        var d = (o.transform.position - center.transform.position).magnitude;
        var a = Mathf.Acos(radius / d);
        return new Vector3(Mathf.Cos(angle - a) * radius + center.transform.position.x, 0, Mathf.Sin(angle - a) * radius + center.transform.position.z);
    }

    public static Vector3 getTangentPointCounter(GameObject center, GameObject radiusMark, GameObject o)
    {
        var radius = (center.transform.position - radiusMark.transform.position).magnitude;
        var angle = getAngle(center, o);
        var d = (o.transform.position - center.transform.position).magnitude;
        var a = Mathf.Acos(radius / d);
        return new Vector3(Mathf.Cos(angle + a) * radius + center.transform.position.x, 0, Mathf.Sin(angle + a) * radius + center.transform.position.z);
    }

}

