using UnityEngine;
using System.Collections;
using System.Linq;

public delegate void VoidFunction();

public class ScriptAction {

    protected static int idIncr = 0;
    protected int id = ++idIncr;

    public ScriptAction nextAction;
    public float waitAfter;
    public float waitBefore;

    private float waitingAfter;
    private float waitingBefore;

    public void Start() {
        if (waitBefore > 0)
        {
            waitingBefore = waitBefore;
            Debug.Log(id + " waiting before: " + waitBefore);
        }
        else {
            Debug.Log(id + " Start without waiting");
            StartAction();
        }
    }

    protected virtual void StartAction() { }
    protected virtual void UpdateAction() { }

    public void Update()
    {
        if (waitingBefore > 0)
        {
            Debug.Log(id + " waiting..." + waitingBefore);
            waitingBefore -= Time.deltaTime;
            if (waitingBefore <= 0)
            {
                Debug.Log(id + " Start after waiting");
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
                onComplete();
            }
        }
        else
        {
            UpdateAction();
        }
        
    }

    // called to get the result of this action or end it for fast forwarding
    public virtual void Instant() { }

    protected static GameObject getMark(string name)
    {
        var marks = GameObject.FindGameObjectsWithTag("Mark");
        return marks.Where(o => o.name == name).FirstOrDefault();
    }

    public VoidFunction onComplete;

    /// <summary>
    /// Called by implementing classes when their action is done
    /// </summary>
    protected void complete()
    {
        if (waitAfter != 0)
        {
            waitingAfter = waitAfter;
        }
        else
        {
            onComplete();
        }
    }
}
