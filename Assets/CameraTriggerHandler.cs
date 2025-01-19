using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTriggerHandler : MonoBehaviour
{
    Camera mainCamera;
    CameraFollow mainCameraFollow;

    public CameraState newState;
    public float leftBounds = 0.0f;
    public float rightBounds = 0.0f;
    public float topBounds = 0.0f;
    public float bottomBounds = 0.0f;

    public Vector2 followOffset;
    public float zoomFactor = 1f;

    [Header("SkyBox")]
    public int skyboxScale;
    public GameObject skybox;

    private CameraFollow camFollow;

    private void Start()
    {
        mainCamera = Camera.main;
        camFollow = Camera.main.gameObject.GetComponent<CameraFollow>();
        mainCameraFollow = mainCamera.gameObject.GetComponent<CameraFollow>();

    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //skybox.GetComponent<SkyboxHelper>().scale = skyboxScale;
            camFollow.zoomFactor = zoomFactor;
            camFollow.offset = followOffset;
            mainCameraFollow.SetCameraState(newState, leftBounds, topBounds, rightBounds, bottomBounds);
        }
    }
}
