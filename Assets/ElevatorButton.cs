using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorButton : MonoBehaviour
{
    public bool canActivate = false;
    public ElevatorScript elevatorScript;

    public void OnTriggerEnter2D(Collider2D other)
    {
        canActivate = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        canActivate = false;
    }

    public void Update()
    {
        if (Input.GetKeyDown("e"))
        {
            elevatorScript.activatedButton = true;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }
}
