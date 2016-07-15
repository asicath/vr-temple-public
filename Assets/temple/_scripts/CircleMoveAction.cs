using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleMoveAction : ScriptAction
{

    public GameObject actor;
    public string centerMarkName;
    public string targetMarkName;
    public string radiusMarkName;
    public float speed;

    public float? targetDegree;

    private GameObject center;
    private GameObject target;
    private GameObject entry;
    private GameObject exit;
    private GameObject radiusMark;

    private bool moveToCircleComplete = false;
    private bool moveOnCircleComplete = false;
    private bool moveFromCircleComplete = false;
    private bool finalRotateComplete = false;

    private float rotationSpeed = 50;



    // Use this for initialization
    protected override void StartAction()
    {
        center = getMark(centerMarkName);
        target = getMark(targetMarkName);
        radiusMark = getMark(radiusMarkName);

        createEntryPoint();
        createExitPoint();
    }

    private void createEntryPoint()
    {

        // determine entry point
        entry = new GameObject("temp entry");

        // determine if we can to move to tangent
        var distance = getDistance(actor, center);
        var radius = getDistance(center, radiusMark);
        if (distance > radius)
        {
            var tangent = getTangentPoint(center, radiusMark, actor);

            var currentAngle = getAngle(center, actor);
            var targetAngle = getAngle(center, target);
            var tangentAngle = getAngle(center.transform.position, tangent);

            if (targetAngle > currentAngle) targetAngle -= Mathf.PI * 2;
            if (tangentAngle > currentAngle) tangentAngle -= Mathf.PI * 2;

            if (tangentAngle > targetAngle)
            {
                entry.transform.position = tangent;
                return;
            }
        }

        // default is just to move to the closest point on circle
        var close = getClosestPointOnCircle(center, radiusMark, actor);
        entry.transform.position = close;
    }

    private void createExitPoint()
    {
        exit = new GameObject("temp exit");

        // determine if we can cut out earlier
        var distance = getDistance(target, center);
        var radius = getDistance(center, radiusMark);
        if (distance > radius)
        {
            var tangent = getTangentPointCounter(center, radiusMark, target);

            var entryAngle = getAngle(center, entry);
            var targetAngle = getAngle(center, target);
            var tangentAngle = getAngle(center.transform.position, tangent);

            if (targetAngle > entryAngle) targetAngle -= Mathf.PI * 2;
            if (tangentAngle > entryAngle) tangentAngle -= Mathf.PI * 2;

            if (tangentAngle > targetAngle)
            {
                exit.transform.position = tangent;
                return;
            }
        }


        // default if just the closest point
        exit.transform.position = getClosestPointOnCircle(center, radiusMark, target);
    }

    /// <summary>
    /// Gets angle in radians
    /// </summary>
    private static float getAngle(GameObject centerMark, GameObject o)
    {
        return getAngle(centerMark.transform.position, o.transform.position);
    }

    private static float getAngle(Vector3 center, Vector3 position)
    {
        var a = Mathf.Atan2(position.z - center.z, position.x - center.x);

        // Lets not deal with negative angles just to make this simple
        if (a < 0) a += Mathf.PI * 2;

        return a;
    }

    private static float getDistance(GameObject a, GameObject b)
    {
        return (a.transform.position - b.transform.position).magnitude;
    }

    private static Vector3 getClosestPointOnCircle(GameObject center, GameObject radiusMark, GameObject o)
    {
        var radius = (center.transform.position - radiusMark.transform.position).magnitude;
        var angle = getAngle(center, o);
        return new Vector3(Mathf.Cos(angle) * radius + center.transform.position.x, 0, Mathf.Sin(angle) * radius + center.transform.position.z);
    }

    // gets the clockwise tangent point from position o
    private static Vector3 getTangentPoint(GameObject center, GameObject radiusMark, GameObject o)
    {
        var radius = (center.transform.position - radiusMark.transform.position).magnitude;
        var angle = getAngle(center, o);
        var d = (o.transform.position - center.transform.position).magnitude;
        var a = Mathf.Acos(radius / d);
        return new Vector3(Mathf.Cos(angle - a) * radius + center.transform.position.x, 0, Mathf.Sin(angle - a) * radius + center.transform.position.z);
    }

    private static Vector3 getTangentPointCounter(GameObject center, GameObject radiusMark, GameObject o)
    {
        var radius = (center.transform.position - radiusMark.transform.position).magnitude;
        var angle = getAngle(center, o);
        var d = (o.transform.position - center.transform.position).magnitude;
        var a = Mathf.Acos(radius / d);
        return new Vector3(Mathf.Cos(angle + a) * radius + center.transform.position.x, 0, Mathf.Sin(angle + a) * radius + center.transform.position.z);
    }

    private float convertAngleToUnity(float angle)
    {
        var a = angle - Mathf.PI * 0.5f;
        if (a < 0) a += Mathf.PI * 2;
        return Mathf.PI * 2 - a;
    }

    private bool rotateToMatch(GameObject mark)
    {
        var angleDelta = target.transform.rotation.eulerAngles - actor.transform.rotation.eulerAngles;
        var direction = angleDelta.normalized;

        if (angleDelta.magnitude > 180) direction *= -1;

        var rotationAmount = 100 * Time.deltaTime;

        if (angleDelta.magnitude > rotationAmount)
        {
            var angle = actor.transform.rotation.eulerAngles + direction * rotationAmount;
            actor.transform.rotation = Quaternion.Euler(angle);
            return false;
        }
        else
        {
            actor.transform.rotation = target.transform.rotation;
            return true;
        }
    }

    private bool rotateToFaceMark(GameObject mark)
    {
        // get the degree from mark to the actor
        var degrees = convertAngleToUnity(getAngle(actor, mark)) * Mathf.Rad2Deg;

        // this is the angle that it should face
        var shouldFace = new Vector3(0, degrees, 0);

        // determine the difference in angles
        var rotationAngleDelta = shouldFace - actor.transform.rotation.eulerAngles;

        var direction = rotationAngleDelta.normalized;
        if (rotationAngleDelta.magnitude > 180) direction *= -1;

        var rotationAmount = rotationSpeed * Time.deltaTime;

        if (rotationAngleDelta.magnitude > rotationAmount)
        {
            //set a smaller angle and exit without moving
            var a = actor.transform.rotation.eulerAngles + direction * rotationAmount;
            actor.transform.rotation = Quaternion.Euler(a);
            return false;
        }
        else
        {
            // set the rotation without incident
            actor.transform.rotation = Quaternion.Euler(shouldFace);
            return true;
        }
    }

    private bool moveToMark(GameObject mark)
    {
        var d = new Vector3(mark.transform.position.x, 0, mark.transform.position.z) - new Vector3(actor.transform.position.x, 0, actor.transform.position.z);

        var moveAmount = speed * Time.deltaTime;

        if (d.magnitude >= moveAmount)
        {
            var n = actor.transform.position + d.normalized * moveAmount;
            actor.transform.position = new Vector3(n.x, actor.transform.position.y, n.z);
            return false;
        }
        else
        {
            actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
            return true;
        }
    }

    private bool rotateAndMoveToMark(GameObject mark)
    {
        // first deal with rotation
        var rotationComplete = rotateToFaceMark(mark);
        if (!rotationComplete) return false;

        // now move
        return moveToMark(mark);
    }

    private bool rotateToTangent(float angle)
    {
        // detemine the ideal rotation
        var degrees = (convertAngleToUnity(angle) + Mathf.PI * 0.5f) % (Mathf.PI * 2);
        var shouldFace = new Vector3(0, degrees * Mathf.Rad2Deg, 0);

        // detemine if we need to move a lesser amount
        var rotationAngleDelta = shouldFace - actor.transform.rotation.eulerAngles;

        // determine closest direction
        var direction = rotationAngleDelta.normalized;
        if (rotationAngleDelta.magnitude > 180) direction *= -1;

        var rotationAmount = rotationSpeed * Time.deltaTime;

        if (rotationAngleDelta.magnitude > rotationAmount)
        {
            //set a smaller angle and exit without moving
            var a = actor.transform.rotation.eulerAngles + direction * rotationAmount;
            actor.transform.rotation = Quaternion.Euler(a);
            return false;
        }
        else
        {
            // set the rotation without incident
            actor.transform.rotation = Quaternion.Euler(shouldFace);
            return true;
        }
    }

    private bool setPositionOnCircle()
    {
        // get current degree
        var angle = getAngle(center, actor);
        var targetAngle = getAngle(center, exit);
        var radius = getDistance(center, radiusMark);

        // determine how much angle is covered at current speed/circum
        var circumferance = radius * Mathf.PI * 2;
        var angleDelta = ((speed * Time.deltaTime) / circumferance) * Mathf.PI * 2;
        angleDelta *= -1; // make it deosil

        // propose a new angle
        var nextAngle = angle + angleDelta;


        // make sure we are facing the tangent
        var rotateComplete = rotateToTangent(nextAngle);
        if (!rotateComplete) return false;


        var isComplete = false;
        // determine if the angle oversteps
        if (angle == targetAngle || angle > targetAngle && nextAngle <= targetAngle || angle < targetAngle && nextAngle >= targetAngle)
        {
            //Debug.Log("moveOnCircleComplete");
            isComplete = true;
            nextAngle = targetAngle;
        }

        // last move to the new position
        actor.transform.position = new Vector3(Mathf.Cos(nextAngle) * radius + center.transform.position.x, actor.transform.position.y, Mathf.Sin(nextAngle) * radius + center.transform.position.z);
        return isComplete;
    }

    protected override void UpdateAction()
    {

        if (!moveToCircleComplete)
        {
            moveToCircleComplete = rotateAndMoveToMark(entry);
        }
        else if (!moveOnCircleComplete)
        {
            moveOnCircleComplete = setPositionOnCircle();
        }
        else if (!moveFromCircleComplete)
        {
            moveFromCircleComplete = rotateAndMoveToMark(target);
        }
        else if (!finalRotateComplete)
        {
            finalRotateComplete = rotateToMatch(target);
        }
        else
        {
            // and complete the action
            complete();
        }

    }

    public override void Instant()
    {
        target = getMark(targetMarkName);
        actor.transform.position = new Vector3(target.transform.position.x, actor.transform.position.y, target.transform.position.z);
        actor.transform.rotation = target.transform.rotation;
    }

}
