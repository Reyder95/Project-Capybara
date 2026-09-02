using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backgrounds : MonoBehaviour
{
    public List<SkyboxHelper> backgrounds = new List<SkyboxHelper>();
    public SkyboxHelper GetBackground(int index)
    {
        if (backgrounds.Count > index)
        {
            return backgrounds[index];
        }

        return null;
    }
}
