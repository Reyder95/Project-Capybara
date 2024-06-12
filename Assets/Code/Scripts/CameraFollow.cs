using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    [Range(1, 10)]
    public float smoothFactor;

    private float leftBound = -10.79f;
    private float rightBound = 200.43f;
    private float topBound = 25f;
    private float bottomBound = -5.10f;

    private void LateUpdate()
    {
        Follow();
    }

    void Follow()
    {
        float height = 2f * Camera.main.orthographicSize;
        float width = height * Camera.main.aspect;

        Vector3 targetPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.deltaTime);
        smoothedPosition.x = Mathf.Clamp(smoothedPosition.x, leftBound + width / 2f, rightBound - width / 2f);
        smoothedPosition.y = Mathf.Clamp(smoothedPosition.y, bottomBound + height / 2f, topBound - height / 2f);
        transform.position = smoothedPosition;
    }
}
