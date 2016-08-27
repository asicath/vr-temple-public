using UnityEngine;
using System;
using System.Collections;

public class Ritualist : MonoBehaviour {

    private static GameObject handPrefab;
    private static GameObject loadHand()
    {

        if (handPrefab != null) return Instantiate(handPrefab);

        var path = "Hand";
        var prefab = Resources.Load<GameObject>(path);

        if (prefab == null) throw new Exception("can't find resource: " + path);

        handPrefab = prefab;

        return Instantiate(prefab);
    }



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
    private GameObject leftHandIcon, rightHandIcon;

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
        rightHand.transform.localPosition = new Vector3(0.2f, 0.8f, 0.3f);
        rightHand.transform.localRotation = Quaternion.identity;

        leftHand = new GameObject("left hand");
        leftHand.transform.parent = gameObject.transform;
        leftHand.transform.localPosition = new Vector3(-0.2f, 0.8f, 0.3f);
        leftHand.transform.localRotation = Quaternion.identity;

        // now some icons for them
        leftHandIcon = loadHand();
        leftHandIcon.transform.parent = leftHand.transform;
        leftHandIcon.transform.localPosition = new Vector3(0, 0, 0);

        rightHandIcon = loadHand();
        rightHandIcon.transform.parent = rightHand.transform;
        rightHandIcon.transform.localPosition = new Vector3(0, 0, 0);

    }



}
