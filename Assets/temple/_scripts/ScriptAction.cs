using UnityEngine;
using System.Collections;
using System.Linq;

public delegate void VoidFunction();

public class ScriptAction {

    public ScriptAction nextAction;
    public float waitAfter;

    private float waiting = 0;

    public virtual void Start() { }

    public virtual void UpdateAction() { }

    public void Update()
    {

        if (waiting > 0)
        {
            waiting -= Time.deltaTime;
            if (waiting <= 0)
            {
                waiting = 0;
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
            waiting = waitAfter;
        }
        else
        {
            onComplete();
        }
    }
}
