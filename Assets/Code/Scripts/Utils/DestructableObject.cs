using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructableObject : MonoBehaviour
{
    private Animator animator;
    private BoxCollider2D[] colliders;

    void Start()
    {
        animator = GetComponent<Animator>();
        colliders = GetComponents<BoxCollider2D>();
    }

    public void PlayDeath()
    {
        animator.SetTrigger("Die");
    }

    public void DestroySelf()
    {
        Debug.Log("DESTROYING");
        Destroy(this.gameObject);
    }

    public void RemoveCollider()
    {
        foreach (BoxCollider2D collider in colliders)
        {
            collider.enabled = false;
        }
    }
}
