using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class Move
{

    #region math functions

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

    #endregion

    public static void setPosition(GameObject actor, GameObject mark)
    {
        actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
    }

    public static void setRotation(GameObject actor, GameObject mark)
    {
        actor.transform.rotation = mark.transform.rotation;
    }

    public static void setRotation(GameObject actor, Vector3 vector)
    {
        actor.transform.rotation = Quaternion.Euler(vector);
    }

    public static bool moveToMark(GameObject actor, GameObject mark, float speed)
    {
        var d = new Vector3(mark.transform.position.x, 0, mark.transform.position.z) - new Vector3(actor.transform.position.x, 0, actor.transform.position.z);

        var remainingDistance = d.magnitude;
        var maxMoveAmount = speed * Time.deltaTime;

        if (remainingDistance >= maxMoveAmount)
        {
            var n = actor.transform.position + d.normalized * maxMoveAmount;
            actor.transform.position = new Vector3(n.x, actor.transform.position.y, n.z);
            return false; // not complete yet
        }
        else
        {
            actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
            return true; // complete
        }
    }

    public static bool rotateToFaceMark(GameObject actor, GameObject mark, float speed)
    {
        // get the degree from mark to the actor
        var degrees = (Move.convertAngleToUnity(Move.getAngle(actor, mark)) * Mathf.Rad2Deg) % 360;

        // this is the angle that it should face
        var shouldFace = new Vector3(0, degrees, 0);

        

        // determine the difference in angles
        var rotationAngleDelta = shouldFace - actor.transform.rotation.eulerAngles;

        Debug.Log("rotate: " + degrees + " delta:" + rotationAngleDelta);

        var direction = rotationAngleDelta.normalized;
        if (rotationAngleDelta.magnitude > 180) direction *= -1;

        var rotationAmount = speed * Time.deltaTime;

        if (rotationAngleDelta.magnitude > rotationAmount)
        {
            Debug.Log("part");
            //set a smaller angle and exit without moving
            var a = actor.transform.rotation.eulerAngles + direction * rotationAmount;
            actor.transform.rotation = Quaternion.Euler(a);
            return false;
        }
        else
        {
            Debug.Log("full");
            // set the rotation without incident
            actor.transform.rotation = Quaternion.Euler(shouldFace);
            return true;
        }
    }

    public static bool rotateToMatchMark(GameObject actor, GameObject mark, float speed)
    {
        var angleDelta = mark.transform.rotation.eulerAngles - actor.transform.rotation.eulerAngles;
        var direction = angleDelta.normalized;

        if (angleDelta.magnitude > 180) direction *= -1;

        var rotationAmount = speed * Time.deltaTime;

        if (angleDelta.magnitude > rotationAmount)
        {
            var angle = actor.transform.rotation.eulerAngles + direction * rotationAmount;
            actor.transform.rotation = Quaternion.Euler(angle);
            return false;
        }
        else
        {
            actor.transform.rotation = mark.transform.rotation;
            return true;
        }
    }

    public static bool rotateToMatchVector(GameObject actor, Vector3 vector, float speed)
    {
        var angleDelta = vector - actor.transform.rotation.eulerAngles;
        var direction = angleDelta.normalized;

        if (angleDelta.magnitude > 180) direction *= -1;

        var rotationAmount = speed * Time.deltaTime;

        if (angleDelta.magnitude > rotationAmount)
        {
            var angle = actor.transform.rotation.eulerAngles + direction * rotationAmount;
            actor.transform.rotation = Quaternion.Euler(angle);
            return false;
        }
        else
        {
            actor.transform.rotation = Quaternion.Euler(vector);
            return true;
        }
    }

    public static bool rotateAndMoveToMark(GameObject actor, GameObject mark, float speed, float rotationSpeed)
    {
        // no need to rotate if we are already there
        if (Move.getDistance(actor, mark) == 0) return true;

        // first deal with rotation
        var rotationComplete = Move.rotateToFaceMark(actor, mark, rotationSpeed);
        if (!rotationComplete) return false;

        // now move
        return Move.moveToMark(actor, mark, speed);
    }




}

