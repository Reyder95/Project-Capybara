using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerListOptions : MonoBehaviour
{
    public int spawnerIndex = 0;

    public GameObject getSpawner()
    {
        if (transform.childCount < spawnerIndex + 1)
        {
            spawnerIndex = 0;
        }
        return transform.GetChild(spawnerIndex).gameObject;
    }
}
