using UnityEngine;

using System.Collections;
using InControl;

public class LookAt : MonoBehaviour {

    private Activatable lastEntered = null;

    void Start()
    {
        //Debug.Log(InputManager.ActiveDevice.Name);
        //Debug.Log(InputManager.ActiveDevice.IsAttached);
    }

    void Update()
    {
        if (checkForPress()) doIt();
    }

    bool checkForPress()
    {
        if (InputManager.Devices == null) return false;

        foreach (var i in InputManager.Devices)
        {
            if (i.Action1.WasPressed) return true;

            foreach (var c in i.Controls)
            {
                if (c.WasPressed)
                {
                    //
                    if (c.Handle == "Button 0") return true;
                    else
                    {
                        Debug.Log(c.Handle);
                    }
                }
            }
        }
        return false;
    }

    void doIt()
    {
        if (lastEntered != null)
        {
            Debug.Log("activating " + lastEntered.name);
            lastEntered.onActivate();
            lastEntered = null;
        }
    }


    void OnTriggerEnter(Collider col)
    {
        lastEntered = col.GetComponent<Activatable>();
        if (lastEntered != null)
        {
            lastEntered.onEnter();
        }
    }

    void OnTriggerExit(Collider col)
    {
        var a = col.GetComponent<Activatable>();

        if (a != null)
        {
            lastEntered = null;
            a.onExit();
        }
        
    }



}
