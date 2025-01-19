using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum CameraState
{
    STATIONARY,
    FOLLOW
}

public class CameraFollow : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public float zoomFactor = 1f;
    [Range(1, 10)]
    public float smoothFactor;
    public float initZoom;

    private float leftBound = -10.79f;
    private float rightBound = 200.43f;
    private float topBound = 25f;
    private float bottomBound = -100f;

    private CameraState state = CameraState.FOLLOW;

    private float stationary_leftX = 0.0f;
    private float stationary_rightX = 0.0f;
    private float stationary_topY = 0.0f;
    private float stationary_bottomY = 0.0f;

    private float orthographicEndpoint;
    private Vector3 cameraEndPosition;


    private void Start()
    {
        initZoom = Camera.main.orthographicSize;
    }

    private void LateUpdate()
    {
        if (state == CameraState.FOLLOW)
        {
            Follow();
        }
        else if (state == CameraState.STATIONARY)
        {
            Stationary();
        }
    }

    public void SetCameraState(CameraState newState, float leftBounds, float topBounds, float rightBounds, float bottomBounds)
    {
        this.state = newState;

        if (this.state == CameraState.FOLLOW)
        {
            this.topBound = topBounds;
            this.bottomBound = bottomBounds;
            this.leftBound = leftBounds;
            this.rightBound = rightBounds;
        }
        else if (this.state == CameraState.STATIONARY)
        {
            this.stationary_leftX = leftBounds;
            this.stationary_rightX = rightBounds;
            this.stationary_topY = topBounds;
            this.stationary_bottomY = bottomBounds;
        }
    }

    void Follow()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        Camera.main.orthographicSize = initZoom * zoomFactor;

        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftBound + width / 2f, rightBound - width / 2f);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bottomBound + height / 2f, topBound - height / 2f);
        transform.position = smoothedPosition;
    }

    void Stationary()
    {
        float desiredWidth = stationary_rightX - stationary_leftX;
        float desiredHeight = stationary_topY - stationary_bottomY;

        if (desiredWidth <= 0.0f)
        {
            Debug.Log("Invalid Range!");
            return;
        }

        float aspectRatio = Camera.main.aspect;
        float orthographicSizeByWidth = desiredWidth / (2f * aspectRatio);
        float orthographicSizeByHeight = desiredHeight / 2f;
        orthographicEndpoint = Mathf.Max(orthographicSizeByWidth, orthographicSizeByHeight);
        Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, orthographicEndpoint, 0.8f * Time.deltaTime);

        float centerX = (stationary_leftX + stationary_rightX) / 2f;
        float centerY = (stationary_topY + stationary_bottomY) / 2f;
        cameraEndPosition = new Vector3(centerX, centerY, -10f);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, cameraEndPosition, 0.8f * Time.deltaTime);
    }
}
