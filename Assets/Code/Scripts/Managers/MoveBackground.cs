using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private float yPos;
    public bool adjustY = false;
    private float initCamY;
    public Camera cam;
    [SerializeField] private float parallaxEffect;
    public Vector2 camOffset = new Vector3(0, 0);
    private Vector3 startLocalPos;

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        yPos = startPosY;
        initCamY = cam.transform.position.y;
        startLocalPos = transform.localPosition;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float parentScaleX = transform.parent.lossyScale.x;
        float distance = (cam.transform.position.x * parallaxEffect) / parentScaleX;

        transform.localPosition = new Vector3(
            startLocalPos.x + camOffset.x / parentScaleX + distance,
            startLocalPos.y,
            startLocalPos.z
            );
    }

    public void AdjustY(bool adjustY)
    {
        startPosY = transform.position.y;
        initCamY = cam.transform.position.y;
        this.adjustY = adjustY;
    }

    public void ResetStartPosition()
    {
    }
}
