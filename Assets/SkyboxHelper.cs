using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHelper : MonoBehaviour
{
    public float scale = 1f;
    public bool adjustY = false;
    public Vector3 offset;
    public CameraFollow cameraFollow;
    [SerializeField] private Camera cam;
    [SerializeField] private SpriteRenderer reference;

    private float baselineY;
    private float followOffsetY;
    private bool wasAdjustingY;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Start()
    {
        baselineY = transform.position.y;

        foreach (Transform child in transform)
        {
            MoveBackground background = child.GetComponent<MoveBackground>();
            if (background != null)
                background.camOffset = offset;
        }
    }

    //private void Update()
    //{
    //    float zoomRatio = Camera.main.orthographicSize / cameraFollow.initZoom;
    //    float targetScale = scale * zoomRatio;

    //    transform.position = new Vector2(transform.position.x, transform.position.y);
    //}

    private void LateUpdate()
    {
        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;
        Vector2 native = reference.sprite.bounds.size;
        float cover = Mathf.Max(worldWidth / native.x, worldHeight / native.y);
        cover *= scale;
        transform.localScale = new Vector3(cover, cover, 1f);

        Vector3 p = transform.position;

        if (adjustY)
        {
            if (!wasAdjustingY)
                followOffsetY = transform.position.y - cam.transform.position.y;

            p.y = cam.transform.position.y + followOffsetY;
        }
        else
        {
            if (wasAdjustingY)
                baselineY = p.y;
            p.y = transform.position.y;
        }

        transform.position = p;
        wasAdjustingY = adjustY;
    }

    public void AdjustY(bool enable)
    {
        adjustY = enable;
    }
}
