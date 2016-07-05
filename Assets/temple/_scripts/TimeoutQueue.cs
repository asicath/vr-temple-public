using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;

public delegate void TimeoutFunction();

public class Timeout
{
    public float time;
    private float elapsedTime = 0;
    public TimeoutFunction onTimeout;
    public bool expired = false;

    public void update(float advance)
    {
        if (expired) return;

        elapsedTime += advance;

        if (elapsedTime >= time)
        {
            expired = true;
            onTimeout();
        }
    }

    public void clear()
    {
        expired = true;
    }
}

public class TimeoutQueue : MonoBehaviour
{
    private List<Timeout> timeouts = new List<Timeout>();

    public void FixedUpdate()
    {
        UpdateTimeouts(Time.deltaTime);
    }

    private void UpdateTimeouts(float time)
    {
        bool needsUpdate = false;
        foreach (var o in timeouts.ToList())
        {
            o.update(time);
            if (o.expired) needsUpdate = true;
        }

        if (needsUpdate) timeouts = timeouts.Where(o => !o.expired).ToList();
    }

    protected Timeout setTimeout(float time, TimeoutFunction onTimeout)
    {
        var timeout = new Timeout
        {
            onTimeout = onTimeout,
            time = time
        };
        timeouts.Add(timeout);
        return timeout;
    }   

}
