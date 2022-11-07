using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuperSpeed : PowerUpTimer
{
    public int time = 10;

    public float original_speed;

    public override void Effect(GameObject player)
    {
        this.Start();

        PlayerController playerController = player.GetComponent<PlayerController>();

        // Store initial values
        original_speed = playerController.speed;

        playerController.speed = 20;
    }

    public override void EffectOver(GameObject player)
    {
        this.Stop();

        PlayerController playerController = player.GetComponent<PlayerController>();

        // Revert initial values
        playerController.speed = original_speed;
    }
}
