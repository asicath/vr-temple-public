using UnityEngine;
using System.Collections;

public class Chair : MonoBehaviour {

    public GameObject standPosition;

	// Use this for initialization
	void Start () {
        standPosition = new GameObject();
        standPosition.tag = "Mark";
        standPosition.name = gameObject.name + " Stand";
        standPosition.transform.parent = transform;
        standPosition.transform.localPosition = new Vector3(0, 0, 0.5f);
        standPosition.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //Debug.Log("set location");
    }
	
	// Update is called once per frame
	void Update () {
	    
	}

}
