using UnityEngine;
using System.Collections;

public class AllAction : ScriptAction {

    static public AllAction create(params ScriptAction[] actions)
    {
        return new AllAction { actions = actions };
    }

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
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i].Update();
            if (actions[i].isComplete && actions[i].nextAction != null)
            {
                actions[i] = actions[i].nextAction;
                actions[i].onComplete = completeChildAction;
                actions[i].Start();
            }
        }
    }

    public void completeChildAction(ScriptAction action)
    {
        if (action.nextAction == null) completed++;

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
            var next = action.nextAction;
            while (next != null)
            {
                next.Instant();
                next = next.nextAction;
            }
        }
    }

}
