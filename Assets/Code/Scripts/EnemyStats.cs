using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyStats : MonoBehaviour
{
    public int health = 10;
    public int maxHealth = 10;
    public float speed = 5.0f;
    public TMP_Text healthText;

    private void Start()
    {
        health = maxHealth;
        healthText.text = health + "/" + maxHealth;
    }

    private void Update()
    {
        healthText.text = health + "/" + maxHealth;
    }


    public void DealDamage(int damage)
    {
        health -= damage;
        Debug.Log(damage);

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
