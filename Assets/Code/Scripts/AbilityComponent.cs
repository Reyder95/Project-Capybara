using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityComponent : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other);
        if (other.gameObject.tag == "Destructable")
        {
            if (other.gameObject.TryGetComponent<DestructableObject>(out DestructableObject obj))
            {
                obj.PlayDeath();
            }
            else
            {
                Destroy(other);
            }
        }
    }
}
