using UnityEngine;
using System.Collections;

public class CircleMoveAction : ScriptAction
{

    public GameObject actor;
    //public float degreeEnd;
    public float speed;
    private float radius;
    private GameObject center;
    private GameObject target;
    private GameObject entry;
    public string centerMarkName;
    public string targetMarkName;
    public string radiusMarkName;

    private float angleDelta;

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

        var radiusMark = getMark(radiusMarkName);
        radius = (center.transform.position - radiusMark.transform.position).magnitude;


        // determine entry point
        var angle = getAngleX(center, actor, false);

        entry = new GameObject("temp entry");
        var d = (actor.transform.position - center.transform.position).magnitude;

        if (d < radius)
        {
            entry.transform.position = new Vector3(Mathf.Cos(angle + Mathf.PI) * radius + center.transform.position.x, 0, Mathf.Sin(angle + Mathf.PI) * radius + center.transform.position.z);
        }
        else
        {
            var a = Mathf.Acos(radius / d);
            entry.transform.position = new Vector3(Mathf.Cos(angle - a) * radius + center.transform.position.x, 0, Mathf.Sin(angle - a) * radius + center.transform.position.z);
        }

    }

    /// <summary>
    /// Gets angle in radians
    /// </summary>
    private static float getAngleX(GameObject centerMark, GameObject o, bool noNegatives = true)
    {
        var c = centerMark.transform.position;
        var p = o.transform.position;

        var a = Mathf.Atan2(p.z - c.z, p.x - c.x);

        // Lets not deal with negative angles just to make this simple
        if (noNegatives && a < 0) a += Mathf.PI * 2;

        return a;
    }

    private float convertAngleToUnity(float angle)
    {
        var a = angle - Mathf.PI * 0.5f;
        if (a < 0) a += Mathf.PI * 2;
        return Mathf.PI * 2 - a;
    }

    private bool moveToMark(GameObject mark)
    {

        // get the degree from mark to the actor
        var degrees = convertAngleToUnity(getAngleX(actor, mark)) * Mathf.Rad2Deg;

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
        }
        




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

    protected override void UpdateAction()
    {

        if (!moveToCircleComplete)
        {
            moveToCircleComplete = moveToMark(entry);
            //if (moveToCircleComplete) Destroy(entry);
        }
        else if (!moveOnCircleComplete)
        {
            // get current degree
            var angle = getAngleX(center, actor);
            var targetAngle = getAngleX(center, target);

            // determine how much angle is covered at current speed/circum
            var circumferance = radius * Mathf.PI * 2;
            var angleDelta = ((speed * Time.deltaTime) / circumferance) * Mathf.PI * 2;
            angleDelta *= -1; // make it deosil

            // propose a new angle
            var nextAngle = angle + angleDelta;

            // determine if the angle oversteps
            if (angle == targetAngle || angle > targetAngle && nextAngle <= targetAngle || angle < targetAngle && nextAngle >= targetAngle)
            {
                //Debug.Log("moveOnCircleComplete");
                moveOnCircleComplete = true;
                nextAngle = targetAngle;
            }



            // detemine the ideal rotation
            var degrees = (convertAngleToUnity(nextAngle) + Mathf.PI * 0.5f) % (Mathf.PI * 2);
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
                return;
            }
            else
            {
                // set the rotation without incident
                actor.transform.rotation = Quaternion.Euler(shouldFace);
            }
            

            // last move to the new position
            actor.transform.position = new Vector3(Mathf.Cos(nextAngle) * radius + center.transform.position.x, actor.transform.position.y, Mathf.Sin(nextAngle) * radius + center.transform.position.z);
        }
        else if (!moveFromCircleComplete)
        {
            moveFromCircleComplete = moveToMark(target);
        }
        else if (!finalRotateComplete)
        {
            var angleDelta = target.transform.rotation.eulerAngles - actor.transform.rotation.eulerAngles;
            var direction = angleDelta.normalized;

            if (angleDelta.magnitude > 180) direction *= -1;

            var rotationAmount = 100 * Time.deltaTime;

            if (angleDelta.magnitude > rotationAmount)
            {
                var angle = actor.transform.rotation.eulerAngles + direction * rotationAmount;
                actor.transform.rotation = Quaternion.Euler(angle);
            }
            else
            {
                actor.transform.rotation = target.transform.rotation;
                finalRotateComplete = true;
            }
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
