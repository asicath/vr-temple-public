using UnityEngine;
using System.Collections;

public class SetPositionAction : ScriptAction
{

    public GameObject actor;
    public GameObject mark;
    public Vector3 offset;
    public string markName;

    protected override string getDebugId()
    {
        return "set position " + actor.name + " to " + markName;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        Instant();
        complete();
    }

    public override void Instant()
    {
        mark = getMark(markName);
        actor.SetActive(true);
        actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
        actor.transform.rotation = mark.transform.rotation;
    }

}
