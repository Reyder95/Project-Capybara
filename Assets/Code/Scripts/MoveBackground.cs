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

        transform.position = new Vector3(startPosX + distance, offset, transform.position.z);
    }
}
