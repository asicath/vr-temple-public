using UnityEngine;
using System.Collections;

public class CircleMoveAction : ScriptAction
{

    public GameObject actor;
    public float degreeEnd;
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

    // Use this for initialization
    public override void Start()
    {
        center = getMark(centerMarkName);
        target = getMark(targetMarkName);

        var radiusMark = getMark(radiusMarkName);
        radius = (center.transform.position - radiusMark.transform.position).magnitude;

        // determine entry point
        var angle = getAngle(actor);
        entry = new GameObject("temp entry");
        entry.transform.position = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
    }

    private float getAngle(GameObject o)
    {
        var c = center.transform.position;
        var p = o.transform.position;
        var a = Mathf.Atan2(p.z - c.z, p.x - c.x);
        if (a < 0) a += Mathf.PI * 2;
        return a;
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

    public override void Update()
    {

        if (!moveToCircleComplete)
        {
            moveToCircleComplete = moveToMark(entry);
            //if (moveToCircleComplete) Destroy(entry);
        }
        else if (!moveOnCircleComplete)
        {
            // get current degree
            var angle = getAngle(actor);
            var targetAngle = getAngle(target);

            // determine how much angle is covered at current speed/circum
            var circumferance = radius * Mathf.PI * 2;
            angleDelta = ((-speed * Time.deltaTime) / circumferance) * Mathf.PI * 2;

            // propose a new angle
            var nextAngle = angle + angleDelta;

            // determine if the angle oversteps
            if (angle == targetAngle || angle > targetAngle && nextAngle <= targetAngle || angle < targetAngle && nextAngle >= targetAngle)
            {
                moveOnCircleComplete = true;
                nextAngle = targetAngle;
            }

            // set the position
            actor.transform.position = new Vector3(Mathf.Cos(nextAngle) * radius, actor.transform.position.y, Mathf.Sin(nextAngle) * radius);

            // find new rotation
            var degrees = (nextAngle * -1 / (Mathf.PI * 2)) * 360;
            var shouldFace = new Vector3(0, degrees + 180, 0);
            actor.transform.rotation = Quaternion.Euler(shouldFace);

        }
        else if (!moveFromCircleComplete)
        {
            moveFromCircleComplete = moveToMark(target);
        }
        else
        {
            // and complete the action
            queue.completeAction();
        }



    }

}
