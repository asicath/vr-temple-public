using UnityEngine;
using System.Collections;

public delegate void VoidFunction();

public class ExecuteVoidFunctionAction : ScriptAction
{

    public static ExecuteVoidFunctionAction create(VoidFunction function, string debugMsg)
    {
        return new ExecuteVoidFunctionAction() { function = function, debugMsg = debugMsg };
    }

    public VoidFunction function;
    public string debugMsg;

    protected override string getDebugId()
    {
        return "execute function: " + debugMsg;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        Instant();
        complete();
    }

    public override void Instant()
    {
        function();
    }

}
