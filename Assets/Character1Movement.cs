using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character1Movement : MonoBehaviour
{
    public Transform[] positions; // 0 is spawn, 1 is entrance, 2 is inside salon, 3 is seat
    public float speed = 2f;
    public float rotSpeed = 5f;
    public float stopDist = 0.1f;

    private CharacterController controller;
    private int curTarget = 0;
    private bool turning = false;
    public bool isSeated = false;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        bool allPositionsReached = curTarget >= positions.Length;

        if (!allPositionsReached)
        {
            if (!turning)
            {
                Transform target = positions[curTarget];
                Vector3 direction = target.position - transform.position;
                direction.y = 0f;

                float distance = direction.magnitude;

                if (distance > stopDist)
                {
                    Vector3 moveDir = direction.normalized;
                    controller.Move(moveDir * speed * Time.deltaTime);

                    Quaternion targetRot = Quaternion.LookRotation(moveDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotSpeed * Time.deltaTime);

                }

                else
                {
                    if (curTarget == 1)
                    {
                        turning = true;
                        StartCoroutine(TurnLeft());
                    }
                    else
                    {
                        curTarget++;

                        if (curTarget >= positions.Length)
                            isSeated = true;
                    }    
                }
            }
        }
    }

    IEnumerator TurnLeft()
    {
        Quaternion startRot = transform.rotation;
        Quaternion endRot = startRot * Quaternion.Euler(0, -90f, 0); // -90, aka Left!
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * 1.5f;
            transform.rotation = Quaternion.Slerp(startRot, endRot, t);
            yield return null;
        }

        turning = false;
        curTarget++;
    }
}
