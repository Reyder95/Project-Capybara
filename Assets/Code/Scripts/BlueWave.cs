using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlueWave : Ability
{
    public BlueWave(bool isUpgrade, Image abilityImage, GameObject projectilePrefab) : base(isUpgrade, "Blue Wave", abilityImage, projectilePrefab) {}

    public override void CastAbility()
    {
        GameObject player = Game.Instance.player;

        if (Game.Instance.blueWaveTimer.stopped)
        {

            GameObject instancedProjectile = player.GetComponent<Player>().InstantiateProjectile(ProjectilePrefab);
            instancedProjectile.GetComponent<BlueWaveComponent>().ShootProjectile();

            Game.Instance.blueWaveTimer.StartTimer();

        }

    }
}