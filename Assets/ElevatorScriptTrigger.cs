using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScriptTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            GetComponentInParent<ElevatorScript>().HandleTrigger(other.gameObject);
            this.gameObject.SetActive(false);
        }
    }
}
