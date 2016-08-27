using UnityEngine;
using System.Collections.Generic;

public class ScriptActionQueue {

    private ScriptAction top, bottom;
    private bool isRunning = false;

    public ScriptAction add(ScriptAction action)
    {
        if (top == null)
        {
            top = action;
            bottom = action;
        }
        else {
            bottom.nextAction = action;
            bottom = action;
        }

        return action;
    }

    private void advanceList()
    {
        if (top == bottom)
        {
            top = null;
            bottom = null;
        }
        else
        {
            // move to the next
            top = top.nextAction;
        }
    }

    private void startNextAction()
    {
        if (top != null)
        {
            // pop the top action
            top.onComplete = completeAction;
            isRunning = true;
            top.Start();
            if (top != null && !top.isComplete) top.Update();
        }
    }

    private void endRunningAction()
    {
        isRunning = false;

        advanceList();
        
        startNextAction();
    }
	
	// Update is called once per frame
	public void Update () {
        if (!isRunning) startNextAction();
        else top.Update();
    }

    public void completeAction(ScriptAction action)
    {
        endRunningAction();
    }

    public void fastForwardTo(ScriptAction action)
    {

        while (top != action && top != null)
        {
            isRunning = false;
            top.Instant();
            advanceList();
        }

    }

}
