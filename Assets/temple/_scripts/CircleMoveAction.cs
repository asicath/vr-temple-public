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
    }

    private float getAngle(GameObject o)
    {
        var c = center.transform.position;
        var p = o.transform.position;
        var a = Mathf.Atan2(p.z - c.z, p.x - c.x);
        if (a < 0) a += Mathf.PI * 2;
        return a;
    }

    public override void Update()
    {

        if (!moveToCircleComplete)
        {
            moveToCircleComplete = true;
        }
        else if (!moveOnCircleComplete)
        {
            // get current degree
            var angle = getAngle(actor);
            var goal = getAngle(target);


            var circumferance = radius * Mathf.PI * 2;
            angleDelta = ((-speed * Time.deltaTime) / circumferance) * Mathf.PI * 2;
            var nextAngle = angle + angleDelta;

            var nextPosition = new Vector3(Mathf.Cos(nextAngle) * radius, actor.transform.position.y, Mathf.Sin(nextAngle) * radius);
            
            actor.transform.position = nextPosition;

            var degrees = (nextAngle * -1 / (Mathf.PI * 2)) * 360;
            var shouldFace = new Vector3(0, degrees + 180, 0);
            actor.transform.rotation = Quaternion.Euler(shouldFace);

        }
        else if (!moveFromCircleComplete)
        {
            moveFromCircleComplete = true;
        }
        else
        {
            // and complete the action
            queue.completeAction();
        }



    }

}
