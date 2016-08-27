using UnityEngine;
using System.Collections;

public class HandMotionAction : ScriptAction
{

    public static HandMotionAction raiseAndLower(GameObject ritualist, string handName)
    {
        var points = new Vector3[] { Ritualist.rightHandNormal + new Vector3(0, 0.5f, 0), Ritualist.rightHandNormal };
        return new HandMotionAction() { ritualist = ritualist, handName = handName, points = points };
    }

    public GameObject ritualist;
    public string handName;
    public Vector3[] points;
    public float speed = 0.5f;
    private GameObject hand;
    private int index;
    private Vector3 normal;

    protected override string getDebugId()
    {
        return "hand motion " + ritualist.name;
    }

    // Use this for initialization
    protected override void StartAction()
    {
        if (handName == "left") hand = ritualist.GetComponent<Ritualist>().leftHand;
        if (handName == "right") hand = ritualist.GetComponent<Ritualist>().rightHand;
    }

    protected override void UpdateAction()
    {
        if (index == points.Length) complete();
        else
        {
            var pointComplete = moveTo(hand, points[index], speed);
            if (pointComplete) index += 1;
        }
    }

    public override void Instant()
    {
        hand.transform.localPosition = points[points.Length - 1];
    }

    public static bool moveTo(GameObject o, Vector3 point, float speed)
    {
        var d = point - o.transform.localPosition;

        var remainingDistance = d.magnitude;
        var maxMoveAmount = speed * Time.deltaTime;

        if (remainingDistance >= maxMoveAmount)
        {
            o.transform.localPosition += d.normalized * maxMoveAmount;
            return false; // not complete yet
        }
        else
        {
            o.transform.localPosition = point;
            return true; // complete
        }
    }

}
