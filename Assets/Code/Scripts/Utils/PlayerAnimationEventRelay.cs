using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEventRelay : MonoBehaviour
{
    public void OnActivateMoveFrame()
    {
        GetComponentInParent<PlayerController>().ActivateMove();
    }
}
