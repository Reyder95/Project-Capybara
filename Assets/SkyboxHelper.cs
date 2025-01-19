using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxHelper : MonoBehaviour
{
    public float scale = 1f;
    public Vector3 offset;
    public bool lerp = false;

    public void Start()
    {
        foreach (Transform child in transform)
        {
            MoveBackground background = child.GetComponent<MoveBackground>();

            background.camOffset = offset;
        }
    }

    private void Update()
    {
        if (lerp)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector2(scale, scale), 0.8f * Time.deltaTime);
        }
        else
        {
            transform.localScale = new Vector2(scale, scale);
            lerp = true;
        }
        
        transform.localPosition = offset;
    }
}
