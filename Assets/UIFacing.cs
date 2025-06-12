using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIFacing : MonoBehaviour
{
    private Camera defaultCam;
    private Vector3 offset;

    private void Start()
    {
        defaultCam = Camera.main;

        offset = transform.localPosition; // Offset from parent.
    }

    private void LateUpdate()
    {
        transform.position = transform.parent.position + offset;

        Vector3 dirToCam = defaultCam.transform.position - transform.position;
        dirToCam.y = 0;

        if (dirToCam == Vector3.zero) return;
        Quaternion targetRot = Quaternion.LookRotation(-dirToCam, Vector3.up);
        transform.rotation = targetRot;
    }
}
