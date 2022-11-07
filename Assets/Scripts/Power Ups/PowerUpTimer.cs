using System;
using System.Collections;
using System.Timers;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpTimer : PowerUpBase
{

    public int time = 10;

    public Timer timer = new Timer();
    public bool requestStop = false;

    public PowerUpTimer()
    {
        timer.Interval = 1000;
        timer.Elapsed += OnTimerElapsed;
        timer.AutoReset = false;
    }

    public override abstract void Effect(GameObject player);

    public abstract override void EffectOver(GameObject player);

    public void OnTimerElapsed(object sender, ElapsedEventArgs e)
    {
        Debug.Log(time + " seconds left!");

        time--;

        if (!requestStop)
            timer.Start();
    }

    public void Stop()
    {
        requestStop = true;
    }


    public void Start()
    {
        timer.Start();

        Debug.Log(time + " seconds left!");

        time--;
    }

}
