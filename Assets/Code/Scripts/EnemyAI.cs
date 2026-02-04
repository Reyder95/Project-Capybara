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
    public GameObject sprite;

    public Vector2 leftBounds;
    public Vector2 rightBounds;

    public bool stop = false;

    public LayerMask obstacleLayer;
    public float detectionDistance = 1f;
    public float rayHeight = 0.5f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public float speed = 15f;

    public float jumpForce = 10f;

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
                        this.gameObject.transform.Translate(new Vector2(-speed / 5, 0) * Time.deltaTime);

                        if (gameObject.transform.position.x < leftBounds.x)
                            stop = true;
                    }

                    if (facing == Direction.Right)
                    {
                        this.gameObject.transform.Translate(new Vector2(speed / 5, 0) * Time.deltaTime);

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

                    sprite.gameObject.transform.localScale = new Vector2(sprite.gameObject.transform.localScale.x * -1, sprite.gameObject.transform.localScale.y);

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
                        sprite.gameObject.transform.localScale = new Vector2(sprite.gameObject.transform.localScale.x * -1, sprite.gameObject.transform.localScale.y);
                        facing = Direction.Left;
                    }
                }
                else if (target.transform.position.x > gameObject.transform.position.x)
                {
                    if (facing == Direction.Left)
                    {
                        sprite.gameObject.transform.localScale = new Vector2(sprite.gameObject.transform.localScale.x * -1, sprite.gameObject.transform.localScale.y);
                        facing = Direction.Right;
                    }
                }

                if (facing == Direction.Left)
                {
                    this.gameObject.transform.Translate(new Vector2(-speed / 5, 0) * Time.deltaTime);
                }

                if (facing == Direction.Right)
                {
                    this.gameObject.transform.Translate(new Vector2(speed / 5, 0) * Time.deltaTime);
                }
            }
        }

        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + rayHeight);
        Vector2 rayDirection = facing == Direction.Left ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionDistance, obstacleLayer);

        Debug.DrawRay(rayOrigin, rayDirection * detectionDistance, Color.red);

        if (hit.collider != null)
        {
            if (CanJump())
            {
                Jump();

            }
        }
    }

    bool CanJump()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, obstacleLayer);
    }

    void Jump()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
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
