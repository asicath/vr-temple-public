using UnityEngine;
using System.Collections;
using System.Linq;

public delegate void ActionFunction(ScriptAction action);

public class ScriptAction {

    protected static int idIncr = 0;
    protected int id = ++idIncr;

    public ScriptAction nextAction;
    public float waitAfter;
    public float waitBefore;

    private float waitingAfter;
    private float waitingBefore;

    public bool isComplete = false;

    public void Start() {
        emitDebugLog("start");

        isComplete = false;
        if (waitBefore > 0)
        {
            waitingBefore = waitBefore;
        }
        else {
            StartAction();
        }
    }

    protected virtual void StartAction() { }
    protected virtual void UpdateAction() { }

    public void Update()
    {
        if (waitingBefore > 0)
        {
            waitingBefore -= Time.deltaTime;
            if (waitingBefore <= 0)
            {
                waitingBefore = 0;
                StartAction();
            }
        }
        else if (waitingAfter > 0)
        {
            waitingAfter -= Time.deltaTime;
            if (waitingAfter <= 0)
            {
                waitingAfter = 0;
                isComplete = true;
                onComplete(this);
            }
        }
        else if (!isComplete)
        {
            UpdateAction();
        }
        
    }

    // called to get the result of this action or end it for fast forwarding
    public virtual void Instant() { }

    protected static GameObject getMark(string name)
    {
        if (name == null) return null;
        var marks = GameObject.FindGameObjectsWithTag("Mark");
        if (marks.Length == 0) Debug.Log("can't find mark: " + name);
        return marks.Where(o => o.name == name).FirstOrDefault();
    }

    public ActionFunction onComplete;

    /// <summary>
    /// Called by implementing classes when their action is done
    /// </summary>
    protected void complete()
    {
        emitDebugLog("complete");

        if (waitAfter != 0)
        {
            waitingAfter = waitAfter;
        }
        else
        {
            isComplete = true;
            onComplete(this);
        }
    }

    public ScriptAction then(ScriptAction action)
    {
        if (nextAction == null) nextAction = action;
        else nextAction.then(action);
        return this;
    }

    public void emitDebugLog(string action)
    {
        Debug.Log(getDebugId() + ":" + action);
    }

    protected virtual string getDebugId() {
        return "null";
    }

    
}
