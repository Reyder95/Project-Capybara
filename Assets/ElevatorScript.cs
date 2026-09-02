using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public GameObject destinationObject;
    public GameObject elevatorColliderObject;
    public bool moving = false;
    public bool positioning = false;
    public bool activatedButton = false;
    public GameObject playerPosition;
    public void HandleTrigger(GameObject other)
    {
        if (!activatedButton)
            return;

        other.transform.SetParent(transform);

        if (destinationObject != null)
        {
            positioning = true;
            elevatorColliderObject = other;
        }
    }

    private void Update()
    {
        if (!elevatorColliderObject)
            return;
        PlayerController controller = elevatorColliderObject.GetComponent<PlayerController>();
        if (positioning)
        {


            controller.player.hasControl = false;

            controller.player.velocity = Vector3.zero;

            controller.autoMove = true;

            float distance = Mathf.Abs(elevatorColliderObject.transform.position.x - playerPosition.transform.position.x);

            controller.Move(new Vector2(-2f * Time.deltaTime, 0f), true);

            if (distance < 0.05f)
            {
                positioning = false;
                controller.Flip(1.0f, true);
                moving = true;
            }
        }

        if (moving)
        {
            controller.Move(new Vector2(0, 0), true);
            transform.position = new Vector2(transform.position.x, transform.position.y + 1f * Time.deltaTime);

            if (transform.position.y >= destinationObject.transform.position.y)
            {
                moving = false;
                controller.autoMove = false;
                elevatorColliderObject.transform.SetParent(null);
            }
        }
    }
}
