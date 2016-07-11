using UnityEngine;
using System.Collections.Generic;

public class ScriptActionQueue {

    private ScriptAction top, bottom;
    private float waiting = 0;
    private bool isRunning = false;

    public ScriptAction addToQueue(ScriptAction action)
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

    private void startNextAction()
    {
        if (top != null)
        {
            // pop the top action
            top.onComplete = completeAction;
            isRunning = true;
            top.Start();
        }
    }

    private void endRunningAction()
    {
        waiting = 0;
        isRunning = false;

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

        
        startNextAction();
    }
	
	// Update is called once per frame
	public void Update () {
        if (!isRunning) startNextAction();
        else if (waiting > 0)
        {
            waiting -= Time.deltaTime;
            if (waiting <= 0)
            {
                waiting = 0;
                endRunningAction();
            }
        }
        else top.Update();
    }

    public void completeAction()
    {
        if (top.waitAfter != 0)
        {
            waiting = top.waitAfter;
        }
        else
        {
            endRunningAction();
        }
            
    }

    public void fastForwardTo(ScriptAction action)
    {

        while (top != action && top != null)
        {
            if (isRunning)
            {
                isRunning = false;

                if (waiting > 0)
                {
                    waiting = 0;
                }
                else
                {
                    top.Instant();
                }
            }
            else
            {
                top.Instant();
            }

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


    }

}
