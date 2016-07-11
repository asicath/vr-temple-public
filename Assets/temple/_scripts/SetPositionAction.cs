using UnityEngine;
using System.Collections;

public class SetPositionAction : ScriptAction
{

    public GameObject actor;
    public GameObject mark;
    public Vector3 offset;
    public string markName;

    // Use this for initialization
    public override void Start()
    {
        mark = getMark(markName);
        Instant();
        complete();
    }

    public override void Instant()
    {
        actor.transform.position = new Vector3(mark.transform.position.x, actor.transform.position.y, mark.transform.position.z);
        actor.transform.rotation = mark.transform.rotation;
    }

}
