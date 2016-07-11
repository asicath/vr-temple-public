using UnityEngine;
using System.Collections;

public class AllAction : ScriptAction {

    public ScriptAction[] actions;
    private int completed;

	// Use this for initialization
	public override void Start () {
        completed = 0;
        foreach(var action in actions)
        {
            action.onComplete = completeChildAction;
            Debug.Log("set");
            action.Start();
        }
	}
	
	// Update is called once per frame
	public override void UpdateAction () {
        foreach (var action in actions)
        {
            action.Update();
        }
    }

    public void completeChildAction()
    {
        Debug.Log("complete");
        completed++;
        if (completed == actions.Length)
        {
            complete();
            Debug.Log("all complete");
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
