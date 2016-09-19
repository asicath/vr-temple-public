using UnityEngine;
using System.Collections;
using InControl;

public class Menu : MonoBehaviour {

    public static bool TriggerHideMenu = false;

    private Activatable[] icons;
    private bool visible = false;

	// Use this for initialization
	void Start () {
        icons = GetComponentsInChildren<Activatable>();
        hideIcons();
    }
	
	// Update is called once per frame
	void Update () {

        if (TriggerHideMenu)
        {
            TriggerHideMenu = false;
            hideIcons();
            visible = false;
        }
	    
        else if (!visible && checkForPress())
        {
            visible = true;
            showIcons();

            var cam = GameObject.Find("Camera");
            transform.position = cam.transform.position;
            transform.rotation = cam.transform.rotation;
        }

	}

    void hideIcons()
    {
        foreach (var i in icons)
        {
            i.gameObject.SetActive(false);
            i.onExit();
        }
    }

    void showIcons()
    {
        foreach (var i in icons)
        {
            i.gameObject.SetActive(true);
        }
    }



    bool checkForPress()
    {
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

}
