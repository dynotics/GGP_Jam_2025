using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxMenu : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = .3f;
    public float maxOffset = 0.75f;

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Camera.main == null)
        {
            return;
        }

        Vector3 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 targetPosition = startPosition + (offset * offsetMultiplier);

        Vector3 clampedOffset = Vector3.ClampMagnitude(targetPosition - startPosition, maxOffset);
        Vector3 max = startPosition + clampedOffset;

        transform.position = Vector3.SmoothDamp(transform.position, max, ref velocity, smoothTime);


    }
}
