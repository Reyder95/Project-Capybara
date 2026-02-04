using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;

public class GeneralTimer : MonoBehaviour
{
    private int time = 10;
    public int interval = 1000;
    public int maxTime = 10;

    public Timer timer = new Timer();
    public bool stopped = true;

    // Start is called before the first frame update
    void Start()
    {
        timer.Interval = interval;
        timer.Elapsed += OnTimerElapsed;
        timer.AutoReset = true;
    }

    public void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        Debug.Log(time + " seconds left!");

        time--;

        if (time == 0)
            Stop();
    }

    public void Stop()
    {
        stopped = true;
        timer.Stop();
    }

    public void StartTimer()
    {
        time = maxTime;
        timer.Start();
        stopped = false;

        Debug.Log(time + " seconds left!");

        time--;
    }
}
