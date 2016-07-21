using UnityEngine;
using System.Collections;

public class MoveAction : ScriptAction {

    public GameObject actor;
    public GameObject mark;
    public string markName;
    public float rotationSpeed = 100;

    public float speed;

    private float rotation;
    private bool moveComplete = false;
    private bool rotateComplete = false;
    private bool beforeRotateComplete = false;

    protected override string getDebugId()
    {
        return "move " + actor.name + " to " + markName;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        mark = getMark(markName);
        Debug.Log("starting move");
    }

    protected override void UpdateAction()
    {

        if (!moveComplete) moveComplete = Move.rotateAndMoveToMark(actor, mark, speed, rotationSpeed);
        else if (!rotateComplete) rotateComplete = Move.rotateToMatchMark(actor, mark, rotationSpeed);
        else complete();

    }

    public override void Instant()
    {
        mark = getMark(markName);
        actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
        actor.transform.rotation = mark.transform.rotation;
    }

}
