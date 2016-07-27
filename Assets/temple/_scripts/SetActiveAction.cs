using UnityEngine;
using System.Collections;

public class SetActiveAction : ScriptAction
{

    public static SetActiveAction create(string actorName, bool active)
    {
        return new SetActiveAction() { actorName = actorName, active = active };
    }
    public static SetActiveAction create(GameObject actor, bool active)
    {
        return new SetActiveAction() { actor = actor, active = active };
    }

    public GameObject actor;
    public string actorName;
    public bool active;

    protected override string getDebugId()
    {
        if (actorName != null) return "set active " + actorName + " to " + active;
        return "set active " + actor.name + " to " + active;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        if (actorName != null) actor = GameObject.Find(actorName);

        Instant();
        complete();
    }

    public override void Instant()
    {
        actor.SetActive(active);
    }

}
