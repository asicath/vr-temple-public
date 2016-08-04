using UnityEngine;
using System.Collections;

public class CreateMarkAction : ScriptAction {

    public static CreateMarkAction create(GameObject actor, string name)
    {
        return new CreateMarkAction {actor = actor, name = name, waitAfter = 0, waitBefore = 0 };
    }

    public GameObject actor;
    public string name;

    protected override string getDebugId()
    {
        
        return "createMark at " + actor.name;
    }

    // Use this for initialization
    protected override void StartAction() {
        Instant();
        complete();
    }

    public override void Instant()
    {
        var mark = new GameObject(name);
        mark.tag = "Mark";
        mark.transform.position = actor.transform.position;
        mark.transform.rotation = actor.transform.rotation;
    }

}
