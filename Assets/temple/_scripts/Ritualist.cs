using UnityEngine;
using System.Collections;

public class Ritualist : MonoBehaviour {

    private GameObject _inRightHand;
    public GameObject inRightHand
    {
        get { return this._inRightHand; }
        set
        {
            createHands();
            this._inRightHand = value;
            value.transform.parent = rightHand.transform;
            value.transform.position = Vector3.zero;

            // determine if we've got a hold
            var hold = value.transform.FindChild("hold");
            if (hold != null) value.transform.localPosition = Vector3.zero - hold.position;           
        }
    }

    private GameObject _inLeftHand;
    public GameObject inLeftHand
    {
        get { return this._inLeftHand; }
        set
        {
            createHands();
            this._inLeftHand = value;
            value.transform.parent = leftHand.transform;
            value.transform.localPosition = new Vector3(0, 0, 0);
            //value.transform.localRotation = Quaternion.identity;
        }
    }

    private GameObject leftHand, rightHand;

    // Use this for initialization
    void Start () {

        // create marks
        gameObject.name = gameObject.name.Replace("(Clone)", "");

        // create hands
        createHands();
    }

    private void createHands()
    {
        if (rightHand != null) return;

        rightHand = new GameObject("right hand");
        rightHand.transform.parent = gameObject.transform;
        rightHand.transform.localPosition = new Vector3(0.2f, 0.71f, 0.3f);
        rightHand.transform.localRotation = Quaternion.identity;

        leftHand = new GameObject("left hand");
        leftHand.transform.parent = gameObject.transform;
        leftHand.transform.localPosition = new Vector3(-0.2f, 0.71f, 0.3f);
        leftHand.transform.localRotation = Quaternion.identity;
    }
	
}
