using UnityEngine;
using System.Collections;
using System;

public class Flicker : MonoBehaviour {

    private Light light;

    public float minWaitSeconds, maxWaitSeconds;
    private DateTime nextChangeAt;

    public float minIntensity, maxIntensity;
    private float targetIntensity;

    public float maxChangeAtStep;

	// Use this for initialization
	void Start () {
        light = GetComponentInChildren<Light>();
        change();
    }
	
	// Update is called once per frame
	void Update () {
        if (DateTime.Now > nextChangeAt) change();

        if (light.intensity != targetIntensity)
        {
            var v = targetIntensity - light.intensity;

            // check for done
            if (Math.Abs(v) < maxChangeAtStep) light.intensity = targetIntensity;
            else
            {
                v = maxChangeAtStep * Math.Sign(v);
                light.intensity += v;
            }

        }

	}

    void change()
    {
        // determine when the next change will happen
        var secondsToWait = UnityEngine.Random.RandomRange(minWaitSeconds, maxWaitSeconds);
        nextChangeAt = DateTime.Now.AddSeconds(secondsToWait);

        // change intensity
        targetIntensity = UnityEngine.Random.RandomRange(minIntensity, maxIntensity);

        // change position?
    }

}
