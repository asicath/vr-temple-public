using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using InControl;

public class LookAt : MonoBehaviour {

    private SceneIcon selected = null;
    private bool loading = false;
    private Light light;

    void Start()
    {
        //Debug.Log(InputManager.ActiveDevice.Name);
        //Debug.Log(InputManager.ActiveDevice.IsAttached);

        foreach (var i in InputManager.Devices)
        {
            Debug.Log(i.Name + " controls:" + i.Controls.Count);
            
        }
    }

    void Update()
    {
        if (loading)
        {

        }
        else
        {
            checkForPress();
        }

    }

    void checkForPress()
    {
        foreach (var i in InputManager.Devices)
        {
            if (i.Action1.WasPressed) doIt();

            foreach (var c in i.Controls)
            {
                if (c.WasPressed)
                {
                    //
                    if (c.Handle == "Button 0") doIt();
                    else
                    {
                        Debug.Log(c.Handle);
                    }
                }
            }
        }
    }

    void doIt()
    {
        if (selected != null && !loading)
        {
            light = selected.GetComponentInChildren<Light>();
            light.color = Color.green;
            loading = true;
            SceneManager.LoadSceneAsync(selected.sceneName);
        }
    }


    void OnTriggerEnter(Collider col)
    {
        // do nothing if we are loading
        if (loading) return;

        selected = col.GetComponent<SceneIcon>();
        if (selected != null)
        {
            col.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider col)
    {
        // do nothing if we are loading
        if (loading) return;

        if (selected != null)
        {
            col.transform.GetChild(0).gameObject.SetActive(false);
            selected = null;
        }
        
    }



}
