using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTrigger : MonoBehaviour
{
    public Backgrounds backgrounds;
    private bool triggered = false;
    public bool adjustYTransition = false;
    public int backgroundIndex = 0;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (triggered)
            return;

        if (collision.gameObject.tag == "Player")
        {
            triggered = true;

            SkyboxHelper underground = backgrounds.GetBackground(backgroundIndex);

            underground.AdjustY(true);
        }
    }
}
