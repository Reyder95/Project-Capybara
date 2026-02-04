using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public bool active;

    public void Start()
    {
        startPoint = transform.position;
    }

    public void Update()
    {
        if (active)
        {
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            active = true;
        }
    }
}
