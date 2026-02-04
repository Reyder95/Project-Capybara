using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurpleCrystal : Ability
{
    public PurpleCrystal(bool isUpgrade, Image abilityImage, GameObject projectilePrefab) : base(isUpgrade, "Purple Crystal", abilityImage, projectilePrefab) { }

    public override void CastAbility()
    {
        GameObject player = Game.Instance.player;

        if (Game.Instance.purpleCrystalTimer.stopped)
        {

            GameObject instancedProjectile = player.GetComponent<Player>().InstantiateProjectile(ProjectilePrefab);
            instancedProjectile.GetComponent<PurpleCrystalComponent>().ShootProjectile();

            Game.Instance.purpleCrystalTimer.StartTimer();

        }

    }
}
