using UnityEngine;
using System.Collections;

public class AllAction : ScriptAction {

    public ScriptAction[] actions;
    private int completed;

    // Use this for initialization
    protected override void StartAction() {
        completed = 0;
        foreach (var action in actions)
        {
            action.onComplete = completeChildAction;
            action.Start();
        }
	}

    // Update is called once per frame
    protected override void UpdateAction () {
        //Debug.Log("updating actions" + actions.Length);
        foreach (var action in actions)
        {
            action.Update();
        }
    }

    public void completeChildAction()
    {
        completed++;
        if (completed == actions.Length)
        {
            complete();
        }
            
    }

    public override void Instant()
    {
        foreach (var action in actions)
        {
            action.Instant();
        }
    }

}
