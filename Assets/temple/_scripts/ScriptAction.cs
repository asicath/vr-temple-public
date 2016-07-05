using UnityEngine;
using System.Collections;

public delegate void VoidFunction();

public class ScriptAction {

    public ScriptActionQueue queue;
    public float waitAfter;

    public virtual void Start() { }
    public virtual void Update() { }

}
