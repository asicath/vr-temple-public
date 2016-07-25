using UnityEngine;
using System.Collections;

public class SetActiveAction : ScriptAction
{

    public static SetActiveAction create(GameObject actor, bool active)
    {
        return new SetActiveAction() { actor = actor, active = active };
    }

    public GameObject actor;
    public bool active;

    protected override string getDebugId()
    {
        return "set active " + actor.name + " to " + active;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        Instant();
        complete();
    }

    public override void Instant()
    {
        actor.SetActive(active);
    }

}
