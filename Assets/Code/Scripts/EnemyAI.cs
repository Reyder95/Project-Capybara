using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public enum AITypes
    {
        BasicRanged,
        BasicMelee,
        Keeper
    }

    public enum AIStates
    {
        Patrol,
        Attack
    }

    public enum Direction
    {
        Left,
        Right
    }

    public GameObject target = null;

    public Vector2 patrolCenter;
    public float patrolRange = 10f;
    public AITypes aiType;
    public AIStates aiState = AIStates.Patrol;
    public Direction facing = Direction.Left;

    public Vector2 leftBounds;
    public Vector2 rightBounds;

    public bool stop = false;

    public 

    // Start is called before the first frame update
    void Start()
    {
        patrolCenter = transform.position;
        leftBounds = new Vector2(patrolCenter.x - patrolRange, 0);
        rightBounds = new Vector2(patrolCenter.x + patrolRange, 0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (aiState == AIStates.Patrol)
        {
            if (patrolCenter != null)
            {
                if (!stop)
                {
                    if (facing == Direction.Left)
                    {
                        this.gameObject.transform.Translate(new Vector2(-15f / 5, 0) * Time.deltaTime);

                        if (gameObject.transform.position.x < leftBounds.x)
                            stop = true;
                    }

                    if (facing == Direction.Right)
                    {
                        this.gameObject.transform.Translate(new Vector2(15f / 5, 0) * Time.deltaTime);

                        if (gameObject.transform.position.x > rightBounds.x)
                            stop = true;
                    }
                }
                else
                {
                    if (facing == Direction.Left)
                        facing = Direction.Right;
                    else if (facing == Direction.Right)
                        facing = Direction.Left;

                    this.gameObject.transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);

                    stop = false;
                }
            }
        }
        else if (aiState == AIStates.Attack)
        {
            if (target != null)
            {
                if (target.transform.position.x < gameObject.transform.position.x)
                {
                    if (facing == Direction.Right)
                    {
                        this.gameObject.transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                        facing = Direction.Left;
                    }
                }
                else if (target.transform.position.x > gameObject.transform.position.x)
                {
                    if (facing == Direction.Left)
                    {
                        this.gameObject.transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
                        facing = Direction.Right;
                    }
                }

                if (facing == Direction.Left)
                {
                    this.gameObject.transform.Translate(new Vector2(-15f / 5, 0) * Time.deltaTime);
                }

                if (facing == Direction.Right)
                {
                    this.gameObject.transform.Translate(new Vector2(15f / 5, 0) * Time.deltaTime);
                }
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Attack!!!");
            aiState = AIStates.Attack;
            target = collision.gameObject;
        }
    }
}
