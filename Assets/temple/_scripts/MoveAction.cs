using UnityEngine;
using System.Collections;

public class MoveAction : ScriptAction {

    public GameObject actor;
    public GameObject mark;
    public string markName;

    public float speed;

    private float rotation;
    private bool moveComplete = false;
    private bool rotateComplete = false;
    private bool beforeRotateComplete = false;

    // Use this for initialization
    protected override void StartAction()
    {
        mark = getMark(markName);
        
    }

    protected override void UpdateAction()
    {

        if (!beforeRotateComplete)
        {
            beforeRotateComplete = true;
        }

        else if (!moveComplete)
        {
            var d = new Vector3(mark.transform.position.x, 0, mark.transform.position.z) - new Vector3(actor.transform.position.x, 0, actor.transform.position.z);

            var moveAmount = speed * Time.deltaTime;

            if (d.magnitude >= moveAmount)
            {
                var n = actor.transform.position + d.normalized * moveAmount;
                actor.transform.position = new Vector3(n.x, actor.transform.position.y, n.z);
            }
            else
            {
                actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
                moveComplete = true;
            }
        }
        else if (!rotateComplete)
        {
            var angleDelta = mark.transform.rotation.eulerAngles - actor.transform.rotation.eulerAngles;
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
                actor.transform.rotation = mark.transform.rotation;
                rotateComplete = true;
            }           
        }

        if (moveComplete && rotateComplete)
        {
            // and complete the action
            complete();
        }

    }

    public override void Instant()
    {
        mark = getMark(markName);
        actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
        actor.transform.rotation = mark.transform.rotation;
    }

}
