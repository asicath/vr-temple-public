using UnityEngine;
using System.Collections;

public class Blindfold : MonoBehaviour {

    static public GameObject candidates;
    static private bool isOn = false;

    static public void putOn()
    {
        isOn = true;
        if (candidates != null) candidates.SetActive(true);
    }

    static public void takeOff()
    {
        isOn = false;
        if (candidates != null) candidates.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        candidates = this.gameObject;
        if (isOn) putOn();
        else takeOff();
    }
	
}
