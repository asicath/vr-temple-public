using UnityEngine;
using System.Collections;
using System.Linq;

public delegate void VoidFunction();

public class ScriptAction {

    public ScriptActionQueue queue;
    public float waitAfter;

    public virtual void Start() { }
    public virtual void Update() { }

    protected static GameObject getMark(string name)
    {
        var marks = GameObject.FindGameObjectsWithTag("Mark");
        return marks.Where(o => o.name == name).FirstOrDefault();
    }
}
