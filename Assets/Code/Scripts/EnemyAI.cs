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

    float moveSpeed = 6;
    public float jumpSpeed = 15;
    public float walkSpeed = 6;
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float jumpVelocity;

    public Vector2 leftBounds;
    public Vector2 rightBounds;

    Vector3 velocity;

    public bool stop = false;

    public LayerMask obstacleLayer;
    public float detectionDistance = 1f;
    public float rayHeight = 0.5f;

    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    public float speed = 15f;

    public float jumpForce = 10f;

    float gravity;

    private EnemyController controller;

    // Start is called before the first frame update
    void Start()
    {
        patrolCenter = transform.position;
        controller = GetComponent<EnemyController>();
        leftBounds = new Vector2(patrolCenter.x - patrolRange, 0);
        rightBounds = new Vector2(patrolCenter.x + patrolRange, 0);

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (controller.collisions.below)
            moveSpeed = walkSpeed;
        else
            moveSpeed = jumpSpeed;

        //velocity.y += gravity * Time.deltaTime;

        if (aiState == AIStates.Patrol)
        {
            if (patrolCenter != null)
            {
                if (!stop)
                {
                    if (facing == Direction.Left)
                    {
                        velocity.x = -speed / 5;
                        controller.Move(velocity * Time.deltaTime);

                        if (gameObject.transform.position.x < leftBounds.x)
                            stop = true;
                    }

                    if (facing == Direction.Right)
                    {
                        velocity.x = speed / 5;
                        controller.Move(velocity * Time.deltaTime);

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
                    velocity.x = speed / 5;
                    controller.Move(velocity * Time.deltaTime);
                }

                if (facing == Direction.Right)
                {
                    velocity.x = speed / 5;
                    controller.Move(velocity * Time.deltaTime);
                }
            }
        }

        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y + rayHeight);
        Vector2 rayDirection = facing == Direction.Left ? Vector2.left : Vector2.right;

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, rayDirection, detectionDistance, obstacleLayer);

        Debug.DrawRay(rayOrigin, rayDirection * detectionDistance, Color.red);

        //if (hit.collider != null)
        //{
        //    if (CanJump())
        //    {
        //        Jump();

        //    }
        //}
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
