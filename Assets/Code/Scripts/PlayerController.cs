using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent (typeof(BoxCollider2D))]
public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float speed = 5f;

    public PowerUpBase activePowerUp = new SuperSpeed();

    public Vector2 currentMomentum;

    public bool isOnGround = true;

    public Animator animator;

    public bool flipped = false;

    public Player player;

    public LayerMask groundLayer;

    float slopeAngle;

    BoxCollider2D newCollider;

    RaycastOrigins raycastOrigins;
    public CollisionInfo collisions;

    const float skinWidth = .015f;

    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;

    float horizontalRaySpacing;
    float verticalRaySpacing;

    public LayerMask collisionMask;
    float maxClimbAngle = 80;
    float maxDescendAngle = 80;

    struct RaycastOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;

        public bool climbingSlope;
        public bool descendingSlope;
        public float slopeAngle, slopeAngleOld;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;

            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        player = GetComponent<Player>();

        //activePowerUp.Effect(this.gameObject);

        groundLayer = LayerMask.GetMask("Ground");
    }

    private void Start()
    {
        newCollider = GetComponent<BoxCollider2D>();
        CalculateRaySpacing();
    }

    void HorizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);

            if (hit)
            {

                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

                if (i == 0 && slopeAngle <= maxClimbAngle)
                {
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity, slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxClimbAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    if (collisions.climbingSlope)
                    {
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }

                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            }
        }
    }

    void ClimbSlope(ref Vector3 velocity, float slopeAngle)
    {
        float moveDistance = Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

        if (velocity.y <= climbVelocityY)
        {
            if (velocity.x != 0)
                velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
            if (slopeAngle != 0 && slopeAngle <= maxDescendAngle)
            {
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    print("First One: " + (hit.distance - skinWidth));
                    Debug.Log("Second One: " + (Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)));
                    if (hit.distance - (skinWidth*3) <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x))
                    {
                        Debug.Log("TESTING!");
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;

                        Debug.Log(descendVelocityY);
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendVelocityY;

                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }

    void VerticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if (collisions.climbingSlope)
                {
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);
                }

                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
        }

        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(velocity.x);
            rayLength = Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (slopeAngle != collisions.slopeAngle)
                {
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void UpdateRaycastOrigins()
    {
        Bounds bounds = newCollider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void Move(Vector3 velocity)
    {
        if (player.hasControl)
        {
            Flip(velocity.x);

            UpdateRaycastOrigins();
            collisions.Reset();
            collisions.velocityOld = velocity;

            if (velocity.x != 0)
            {

                HorizontalCollisions(ref velocity);
                animator.SetBool("IsWalk", true);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsIdle", true);
            }

            if (velocity.y != 0)
                VerticalCollisions(ref velocity);

            if (velocity.y < 0)
            {
                DescendSlope(ref velocity);
                Debug.Log("Test!!");
            }
            else
            {
                Debug.Log("No test!");
            }



            transform.Translate(velocity);
        }

    }

    void CalculateRaySpacing()
    {
        Bounds bounds = newCollider.bounds;
        bounds.Expand(skinWidth * -2);

        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

    public void ActivateMove()
    {
        player.hasControl = true;
    }

    private void Flip(float xVelocity)
    {
        if (player.hasControl)
        {
            if (xVelocity < 0)
            {
                animator.SetBool("NoBond", true);
                Vector3 animatorScale = animator.transform.localScale;

                if (animatorScale.x > 0)
                {
                    animatorScale.x *= -1;
                    animator.transform.localScale = animatorScale;
                }
            }
            else if (xVelocity > 0)
            {
                animator.SetBool("NoBond", false);
                Vector3 animatorScale = animator.transform.localScale;

                if (animatorScale.x < 0)
                {
                    animatorScale.x *= -1;
                    animator.transform.localScale = animatorScale;
                }

            }
        }
        //if (player.hasControl)
        //{

        //    flipped = !flipped;

        //    if (!flipped)
        //    {
        //        animator.SetBool("NoBond", false);
        //    }
        //    else
        //    {
        //        animator.SetBool("NoBond", true);
        //    }

        //    Vector3 animatorScale = animator.transform.localScale;
        //    animatorScale.x *= -1;
        //    animator.transform.localScale = animatorScale;
        //}

    }

    private void LateUpdate()
    {

        if (Input.GetKeyDown(KeyCode.F))
        {
            Game.Instance.playerInfo.abilityList[Game.Instance.playerInfo.currAbility].CastAbility();
        }

        if (!player.hasControl && (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S)))
        {
            WakeUp();
        }
    }

    private void WakeUp()
    {
        animator.SetBool("IsWake", true);
    }

    private void Walk(Vector2 dir)
    {
        Vector2 moveDirection = dir;

        if (player.hasControl)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 20f, groundLayer);

            if (slopeAngle != 0)
            {
                float slopeDirection = Mathf.Sign(hit.normal.x);
                float slopeAngleRad = slopeAngle * Mathf.Deg2Rad;
                moveDirection.x = Mathf.Cos(slopeAngleRad) * slopeDirection;
                moveDirection.y = Mathf.Sin(slopeAngleRad);
            }

            float adjustedSpeed = speed * Mathf.Cos(slopeAngle * Mathf.Deg2Rad);

            Debug.Log(adjustedSpeed);

            rb.velocity = moveDirection * adjustedSpeed;
            currentMomentum = rb.velocity;

            if (dir.x != 0)
            {

                animator.SetBool("IsWalk", true);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsIdle", true);
            }
        }

    }
}
