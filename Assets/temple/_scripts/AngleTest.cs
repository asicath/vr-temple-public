using UnityEngine;
using System.Collections;

public class AngleTest : MonoBehaviour {

    class Position
    {
        public float x, y;
    }

	// Use this for initialization
	void Start () {

        var center = new Position { x = 0, y = 0 };

        var lowerRight = new Position { x = 10, y = -10 };
        var upperRight = new Position { x = 10, y = 10 };
        var upperLeft = new Position { x = -10, y = 10 };
        var lowerLeft = new Position { x = -10, y = -10 };

        var angle = Mathf.Atan2(lowerRight.y - center.y, lowerRight.x - center.x);
        if (angle < 0) angle += Mathf.PI * 2;

        // + Mathf.PI * 0.5f

        Debug.Log(angle * Mathf.Rad2Deg);

        Debug.Log(convertToUnity(angle) * Mathf.Rad2Deg);


        var recreate = new Position { x = Mathf.Cos(angle), y = Mathf.Sin(angle) };

        Debug.Log(recreate.x + ", " + recreate.y);
	}

    private float convertToUnity(float angle)
    {
        var a = angle - Mathf.PI * 0.5f;
        if (a < 0) a += Mathf.PI * 2;
        return Mathf.PI * 2 - a;
    }


}
