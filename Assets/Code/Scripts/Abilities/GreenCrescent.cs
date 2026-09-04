using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading.Tasks;

public class GreenCrescent : Ability
{
    public GreenCrescent(bool isUpgrade, Image abilityImage, GameObject projectilePrefab) : base(isUpgrade, "Green Crescent", abilityImage, projectilePrefab) {}

    public override void CastAbility()
    {
        GameObject player = Game.Instance.player;

        if (Game.Instance.greenCrescentTimer.stopped)
        {
            GameObject instancedProjectile = player.GetComponent<Player>().InstantiateProjectile(ProjectilePrefab);
            instancedProjectile.GetComponent<GreenCrescentComponent>().ShootProjectile();

            Game.Instance.greenCrescentTimer.StartTimer();
        }
    }
}