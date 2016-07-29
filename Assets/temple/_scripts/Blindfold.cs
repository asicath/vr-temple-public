using UnityEngine;
using System.Collections;

public class Blindfold : MonoBehaviour {

    static public GameObject candidates;

    static public void putOn()
    {
        candidates.SetActive(true);
    }

    static public void takeOff()
    {
        candidates.SetActive(false);
    }

    // Use this for initialization
    void Start () {
        candidates = this.gameObject;
    }
	
}
