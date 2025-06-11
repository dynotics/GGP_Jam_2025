using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public float walkSpeed = 0.5f;
    public float sprintSpeed = 1f;

    public Transform orientation;
    public Vector3 moveDir;
    public Rigidbody rb;


    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal");
        float inputZ = Input.GetAxisRaw("Vertical");

        moveDir = orientation.forward * inputZ + orientation.right * inputX;

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            walkSpeed = sprintSpeed;

            rb.AddForce(moveDir.normalized * walkSpeed * 10f, ForceMode.Force);
        }

        else
        {
            walkSpeed = 2f;
        }

        rb.AddForce(moveDir.normalized * walkSpeed * 10f, ForceMode.Force);


    }
}
