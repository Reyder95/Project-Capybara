using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    private Rigidbody2D rb;

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    public float speed = 5f;

    public TMPro.TextMeshProUGUI powerUpTimer;

    public PowerUpBase activePowerUp = new SuperSpeed();

    public Vector2 currentMomentum;

    public bool isOnGround = true;

    public Animator animator;

    public bool flipped = false;

    // Start is called before the first frame update
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        activePowerUp.Effect(this.gameObject);

        Debug.Log("Test!");
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");

        Vector2 dir = new Vector2(x, y);

        if (isOnGround)
        {
            if (x != 0)
            {
                

                animator.SetBool("IsWalk", true);
                animator.SetBool("IsIdle", false);
            }
            else
            {
                animator.SetBool("IsWalk", false);
                animator.SetBool("IsIdle", true);
            }

            Walk(dir);
        }

        if (x != 0)
        {
            if (x < 0 && !flipped)
            {
                flipped = true;
                Flip();
            }
            else if (x > 0 && flipped)
            {
                flipped = false;
                Flip();
            }
        }
        


        //if (movement != 0)
        //{
        //    _animator.SetBool("IsWalk", true);
        //    _animator.SetBool("IsIdle", false);
        //}
        //else
        //{
        //    _animator.SetBool("IsWalk", false);
        //    _animator.SetBool("IsIdle", true);
        //}

        //if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        //{
        //    _rigidbody.AddForce(new Vector2(0, JUMP_SPEED), ForceMode2D.Impulse);
        //}


    }

    private void Flip()
    {
        Vector3 animatorScale = animator.transform.localScale;
        animatorScale.x *= -1;    
        animator.transform.localScale = animatorScale;
    }

    private void LateUpdate()
    {
        if (Mathf.Abs(rb.velocity.y) > 0.001f)
        {
            isOnGround = false;
        }
        else
        {
            isOnGround = true;
        }

        if (Input.GetKeyDown(KeyCode.Space) && isOnGround)
        {
            Jump();
        }
        else if (!isOnGround)
        {
            rb.velocity = new Vector2(currentMomentum.x, rb.velocity.y);

            Vector3 newVelocity = rb.velocity;
            float newVelocityX = rb.velocity.x + Input.GetAxis("Horizontal") / 30f;
            newVelocity.x = Mathf.Clamp(newVelocityX, -1 * speed, 1 * speed);
            rb.velocity = newVelocity;
            currentMomentum = rb.velocity;
        }

        if (activePowerUp is PowerUpTimer obj)
        {
            powerUpTimer.text = obj.time.ToString();

            if (obj.time == 0)
            {
                activePowerUp.EffectOver(this.gameObject);
                activePowerUp = null;
            }
        }
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));
        currentMomentum = rb.velocity;
    }

    private void Jump()
    {
        rb.velocity = Vector2.up * 7f;
    }
}
