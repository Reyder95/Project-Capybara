using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUpBase
{

    public abstract void Effect(GameObject player);

    public abstract void EffectOver(GameObject player);
}
