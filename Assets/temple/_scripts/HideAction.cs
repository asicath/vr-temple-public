using UnityEngine;
using System.Collections;

public class HideAction : ScriptAction {

    public GameObject actor;

    protected override string getDebugId()
    {
        return "hide " + actor.name;
    }

    // Use this for initialization
    protected override void StartAction() {
        Instant();
        complete();
    }

    public override void Instant()
    {
        actor.SetActive(false);
    }

}
