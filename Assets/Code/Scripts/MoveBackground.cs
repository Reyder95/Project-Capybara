using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBackground : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private float initCamY;
    public Camera cam;
    [SerializeField] private float parallaxEffect;
    public Vector2 camOffset = new Vector3(0, 0);

    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;
        initCamY = cam.transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (cam.transform.position.x * parallaxEffect);
        float offset = startPosY + (cam.transform.position.y - initCamY);

        transform.position = new Vector3(camOffset.x + startPosX + distance, camOffset.y + offset, transform.position.z);
    }
}
