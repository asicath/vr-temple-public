using UnityEngine;
using System.Collections;

public class MoveAction : ScriptAction {

    public static ScriptAction create(string markName, GameObject actor, float waitAfter = 1f, float waitBefore = 0f)
    {
        return new MoveAction { markName = markName, actor = actor, speed = 2.0f, waitBefore = waitBefore, waitAfter = waitAfter };
    }

    public static ScriptAction createNoRotate(string markName, GameObject actor, float waitAfter = 1f, float waitBefore = 0f)
    {
        return new MoveAction { markName = markName, actor = actor, speed = 2.0f, waitAfter = waitAfter, noRotate = true };
    }


    public GameObject actor;
    public GameObject mark;
    public string markName;
    public float rotationSpeed = 100;

    public bool noRotate = false;

    public float speed;

    private float rotation;
    private bool moveComplete = false;
    private bool rotateComplete = false;
    //private bool beforeRotateComplete = false;

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

        if (!moveComplete)
        {

            if (noRotate) moveComplete = Move.moveToMark(actor, mark, speed);
            //else if (!beforeRotateComplete) beforeRotateComplete = Move.rotateToFaceMark(actor, mark, rotationSpeed);
            else moveComplete = Move.rotateAndMoveToMark(actor, mark, speed, rotationSpeed);
        }
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
