using UnityEngine;
using System.Collections.Generic;

public class ScriptActionQueue : TimeoutQueue {

    private Queue<ScriptAction> actions = new Queue<ScriptAction>();
    private ScriptAction running;
    private bool waiting = false;

	// Use this for initialization
	void Start () {
	    
	}

    public void addToQueue(ScriptAction action)
    {
        action.queue = this;
        actions.Enqueue(action);
    }

    private void startNextAction()
    {
        if (actions.Count > 0)
        {
            running = actions.Dequeue();
            running.Start();
        }
    }

    private void endRunningAction()
    {
        waiting = false;
        running = null;
        startNextAction();
    }
	
	// Update is called once per frame
	void Update () {
        if (running == null) startNextAction();
        else if (!waiting) running.Update();
    }

    public void completeAction()
    {
        if (running.waitAfter != 0)
        {
            waiting = true;
            setTimeout(running.waitAfter, endRunningAction);
        }
        else
        {
            endRunningAction();
        }
            
    }

}
