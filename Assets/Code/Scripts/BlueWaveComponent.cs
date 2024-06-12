using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueWaveComponent : MonoBehaviour
{
    public bool shot = false;
    public Vector3 initPoint;
    public bool flippedShot;
    public bool collided = false;

    public float speed = 0.05f;

    private void Update()
    {
        if (shot)
        {
            if (!flippedShot)
                this.gameObject.transform.Translate(new Vector3(speed, 0, 0) * Time.deltaTime);
            else
            {
                
                this.gameObject.transform.Translate(new Vector3(speed / -1, 0, 0) * Time.deltaTime);
            }
                

            if (Vector3.Distance(initPoint, this.transform.position) > 10.0f)
                RemoveProjectile();
        }

    }

    public void ShootProjectile()
    {
        shot = true;
        flippedShot = Game.Instance.player.GetComponent<PlayerController>().flipped;
        if (flippedShot)
        {
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        
        initPoint = this.gameObject.transform.position;
    }

    public void RemoveProjectile()
    {
        shot = false;
        Destroy(this.gameObject);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.tag == "Enemy")
            {
                RemoveProjectile();
                if (collision.gameObject.TryGetComponent<EnemyStats>(out EnemyStats stats))
                {
                    stats.DealDamage(5);
                }
            }
        }

    }
}
