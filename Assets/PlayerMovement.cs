using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 5f;
    public float sprintSpeed = 10f;

    public float rotSmoothTime = 0.2f;
    public Transform cam;
    private CharacterController controller;
    private float currentAngle;
    private float currentAngleVelocity;

    public bool isFrozen;
    public Camera defaultCam;
    public Camera mirrorCam;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {

        if (isFrozen)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                isFrozen = false;

                if (defaultCam != null)
                {
                    defaultCam.enabled = true;
                }

                if (mirrorCam != null)
                {
                    mirrorCam.enabled = false;
                }
            }

            return;
        }

        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        Vector3 inputDir = new Vector3(inputX, 0f, inputZ).normalized;
        float speed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : walkSpeed;

        if (inputDir.magnitude >= 0.25f)
        {
            float targetAngle = Mathf.Atan2(inputDir.x, inputDir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            currentAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref currentAngleVelocity, rotSmoothTime);
            transform.rotation = Quaternion.Euler(0f, currentAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir * speed * Time.deltaTime);
        }

        else
        {
            controller.Move(Vector3.zero); // Keeps player grounded when stationary.
        }


    }
}
