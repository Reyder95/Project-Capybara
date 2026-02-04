using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Ability 
{
    private bool isUpgrade;
    private string abilityName;
    private Image abilityImage;
    private GameObject projectilePrefab;

    public Ability(bool isUpgrade, string abilityName, Image abilityImage, GameObject projectilePrefab)
    {
        this.isUpgrade = isUpgrade;
        this.abilityName = abilityName;
        this.abilityImage = abilityImage;
        this.projectilePrefab = projectilePrefab;
    }

    public abstract void CastAbility();

    public bool IsUpgrade
    {
        get
        {
            return this.isUpgrade;
        }

        set
        {
            this.isUpgrade = value;
        }
    }

    public string AbilityName
    {
        get
        {
            return this.abilityName;
        }
    }

    public Image AbilityImage
    {
        get
        {
            return this.abilityImage;
        }
    }

    public GameObject ProjectilePrefab
    {
        get
        {
            return this.projectilePrefab;
        }
    }
}