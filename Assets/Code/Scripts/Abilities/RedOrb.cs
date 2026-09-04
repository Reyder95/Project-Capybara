using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RedOrb : Ability
{
    public RedOrb(bool isUpgrade, Image abilityImage, GameObject projectilePrefab) : base(isUpgrade, "Red Orb", abilityImage, projectilePrefab) { }

    public override void CastAbility()
    {
        GameObject player = Game.Instance.player;

        if (Game.Instance.redOrbTimer.stopped)
        {

            GameObject instancedProjectile = player.GetComponent<Player>().InstantiateProjectile(ProjectilePrefab);
            instancedProjectile.GetComponent<RedOrbComponent>().ShootProjectile();

            Game.Instance.redOrbTimer.StartTimer();

        }
    }
}
