using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CircleMoveAction : ScriptAction
{

    static public CircleMoveAction create(string targetMarkName, GameObject actor, float waitBefore = 0, float waitAfter = 1, float speed = 2)
    {
        return new CircleMoveAction { actor = actor, waitBefore = waitBefore, waitAfter = waitAfter, centerMarkName = "Altar", targetMarkName = targetMarkName, radiusMarkName = "Circumabulation", speed = speed };
    }

    static public CircleMoveAction createMoveAround(string targetMarkName, GameObject actor, string centerMarkName, string radiusMarkName, float waitBefore = 0, float waitAfter = 1, float speed = 2)
    {
        return new CircleMoveAction { actor = actor, waitBefore = waitBefore, waitAfter = waitAfter, centerMarkName = centerMarkName, targetMarkName = targetMarkName, radiusMarkName = radiusMarkName, speed = speed };
    }

    static public CircleMoveAction createMoveToDegree(float targetDegree, GameObject actor, float? entryDegree = null, float waitBefore = 0, float waitAfter = 1, float speed = 2)
    {
        return new CircleMoveAction { actor = actor, waitBefore = waitBefore, waitAfter = waitAfter, centerMarkName = "Altar", targetDegree = targetDegree, radiusMarkName = "Circumabulation", speed = speed, entryDegree = entryDegree };
        //return new CircleMoveToDegreeAction { actor = actor, waitAfter = waitAfter, centerMarkName = "Altar", targetDegree = targetDegree, radiusMarkName = "Circumabulation", speed = speed };
    }

    static public CircleMoveAction createMoveDirected(string targetMarkName, GameObject actor, float entryDegree)
    {
        return new CircleMoveAction { actor = actor, waitBefore = 0, waitAfter = 1f, centerMarkName = "Altar", targetMarkName = targetMarkName, radiusMarkName = "Circumabulation", speed = 2, entryDegree = entryDegree };
    }


    public GameObject actor;
    public string centerMarkName;
    public string radiusMarkName;
    public float speed;

    public string targetMarkName;
    public float? targetDegree;

    public float? entryDegree;

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

    protected override string getDebugId()
    {
        if (targetDegree.HasValue) return "move " + actor.name + " to " + targetDegree.Value;
        return "move " + actor.name + " to " + targetMarkName;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        findTarget();

        createEntryPoint();
        createExitPoint();
    }

    private void findTarget()
    {
        // get circle points
        center = getMark(centerMarkName);
        radiusMark = getMark(radiusMarkName);

        // find target
        if (targetMarkName != null) target = getMark(targetMarkName);
        else if (targetDegree.HasValue)
        {
            float angle = Move.convertAngleFromUnity(targetDegree.Value * Mathf.Deg2Rad);
            // determine by degree
            target = new GameObject();
            target.transform.position = getPositionOnCircle(angle);
            target.transform.rotation = getRotationForTangent(angle);
        }
        else
        {
            throw new System.Exception("no target information provided");
        }
    }

    private void createEntryPoint()
    {

        // determine entry point
        entry = new GameObject("temp entry");

        if (entryDegree.HasValue)
        {
            float angle = Move.convertAngleFromUnity(entryDegree.Value * Mathf.Deg2Rad);
            entry.transform.position = getPositionOnCircle(angle);
            entry.transform.rotation = getRotationForTangent(angle);
            return;
        }



        // determine if we can to move to tangent
        var distance = Move.getDistance(actor, center);
        var radius = Move.getDistance(center, radiusMark);
        if (distance > radius)
        {
            var tangent = Move.getTangentPoint(center, radiusMark, actor);

            var currentAngle = Move.getAngle(center, actor);
            var targetAngle = Move.getAngle(center, target);
            var tangentAngle = Move.getAngle(center.transform.position, tangent);

            if (targetAngle > currentAngle) targetAngle -= Mathf.PI * 2;
            if (tangentAngle > currentAngle) tangentAngle -= Mathf.PI * 2;

            if (tangentAngle > targetAngle)
            {
                entry.transform.position = tangent;
                return;
            }
        }

        // default is just to move to the closest point on circle
        var close = Move.getClosestPointOnCircle(center, radiusMark, actor);
        entry.transform.position = close;
    }

    private void createExitPoint()
    {
        
        // if we are going for a particular degree, that is the exit point
        if (targetDegree != null)
        {
            exit = target;
            return;
        }

        exit = new GameObject("temp exit");

        // determine if we can cut out earlier
        var distance = Move.getDistance(target, center);
        var radius = Move.getDistance(center, radiusMark);
        if (distance > radius)
        {
            var tangent = Move.getTangentPointCounter(center, radiusMark, target);

            var entryAngle = Move.getAngle(center, entry);
            var targetAngle = Move.getAngle(center, target);
            var tangentAngle = Move.getAngle(center.transform.position, tangent);

            if (targetAngle > entryAngle) targetAngle -= Mathf.PI * 2;
            if (tangentAngle > entryAngle) tangentAngle -= Mathf.PI * 2;

            if (tangentAngle > targetAngle)
            {
                exit.transform.position = tangent;
                return;
            }
        }


        // default if just the closest point
        exit.transform.position = Move.getClosestPointOnCircle(center, radiusMark, target);
    }

    public static bool rotateToTangent(GameObject actor, float angle, float rotationSpeed)
    {
        // detemine the ideal rotation
        var degrees = (Move.convertAngleToUnity(angle) + Mathf.PI * 0.5f) % (Mathf.PI * 2);
        var shouldFace = new Vector3(0, degrees * Mathf.Rad2Deg, 0);
        return Move.rotateToMatchVector(actor, shouldFace, rotationSpeed);
    }

    private Quaternion getRotationForTangent(float angle)
    {
        // detemine the ideal rotation
        var degrees = (Move.convertAngleToUnity(angle) + Mathf.PI * 0.5f) % (Mathf.PI * 2);
        var shouldFace = new Vector3(0, degrees * Mathf.Rad2Deg, 0);

        return Quaternion.Euler(shouldFace);
    }

    private bool setPositionOnCircle()
    {
        // get current degree
        var angle = Move.getAngle(center, actor);
        var targetAngle = Move.getAngle(center, exit);
        var radius = Move.getDistance(center, radiusMark);

        // determine how much angle is covered at current speed/circum
        var circumferance = radius * Mathf.PI * 2;
        var angleDelta = ((speed * Time.deltaTime) / circumferance) * Mathf.PI * 2;
        angleDelta *= -1; // make it deosil

        // propose a new angle
        var nextAngle = angle + angleDelta;


        // make sure we are facing the tangent
        var rotateComplete = rotateToTangent(actor, nextAngle, rotationSpeed);
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

    private Vector3 getPositionOnCircle(float angle)
    {
        var radius = Move.getDistance(center, radiusMark);
        return new Vector3(Mathf.Cos(angle) * radius + center.transform.position.x, 0, Mathf.Sin(angle) * radius + center.transform.position.z);
    }

    protected override void UpdateAction()
    {

        if (!moveToCircleComplete)
        {
            moveToCircleComplete = Move.rotateAndMoveToMark(actor, entry, speed, rotationSpeed);
        }

        if (moveToCircleComplete && !moveOnCircleComplete)
        {
            moveOnCircleComplete = setPositionOnCircle();
        }

        if (moveOnCircleComplete && !moveFromCircleComplete)
        {
            if (targetDegree.HasValue)
            {
                moveFromCircleComplete = true;
            }
            else
            {
                moveFromCircleComplete = Move.rotateAndMoveToMark(actor, target, speed, rotationSpeed);
            }
        }

        if (moveFromCircleComplete && !finalRotateComplete)
        {
            if (targetDegree.HasValue)
            {
                finalRotateComplete = true;
            }
            else
            {
                finalRotateComplete = Move.rotateToMatchMark(actor, target, rotationSpeed);
            }
        }

        if (finalRotateComplete)
        {
            // and complete the action
            complete();
        }

    }

    public override void Instant()
    {
        findTarget();
        actor.transform.position = new Vector3(target.transform.position.x, actor.transform.position.y, target.transform.position.z);
        actor.transform.rotation = target.transform.rotation;
    }

}
