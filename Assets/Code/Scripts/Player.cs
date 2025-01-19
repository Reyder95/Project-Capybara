using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof (PlayerController))]
public class Player : MonoBehaviour
{
    PlayerController controller;

    public bool hasControl = false;

    public int maxHealth = 100;
    public int currHealth;

    public List<Ability> abilityList = new List<Ability>();
    public int currAbility = 0;

    float gravity;
    float moveSpeed = 6;
    public float jumpSpeed = 15;
    public float walkSpeed = 6;
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    float accelerationTimeAirborne = .0f;
    float accelerationTimeGrounded = .0f;

    public void InitializePlayer()
    {
        currHealth = maxHealth;

        // Debug: Give player all abilities
        //abilityList.Add(Game.Instance.abilityDictionary["Blue Wave"]);
        //abilityList.Add(Game.Instance.abilityDictionary["Green Crescent"]);
        //abilityList.Add(Game.Instance.abilityDictionary["Red Orb"]);
        //abilityList.Add(Game.Instance.abilityDictionary["Purple Crystal"]);
    }

    private void Start()
    {
        controller = GetComponent<PlayerController>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
    }

    private void Update()
    {
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (Input.GetKeyDown(KeyCode.Space) && controller.collisions.below)
        {
            velocity.y = jumpVelocity;
        }

        if (controller.collisions.below)
            moveSpeed = walkSpeed;
        else
            moveSpeed = jumpSpeed;

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);

        if (abilityList[currAbility] != null)
        {
            if (Game.Instance.abilityImage.color != abilityList[currAbility].AbilityImage.color)
                Game.Instance.abilityImage.color = abilityList[currAbility].AbilityImage.color;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            if (currAbility + 1 < abilityList.Count)
                currAbility++;
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (currAbility - 1 >= 0)
               currAbility--;
        }



    }

    public GameObject InstantiateProjectile(GameObject projectile)
    {
        Vector3 shooter = this.gameObject.transform.GetChild(0).transform.position;
        GameObject instancedProjectile = Instantiate(projectile, shooter, Quaternion.identity);

        return instancedProjectile;
    }
}
