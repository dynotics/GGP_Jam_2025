using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum CustomerState
{
    WalkingReception,
    WaitReception,
    WalkingSeat,
    Seated,
    Finished,
    Leaving
}

public enum HairStyle
{
    A,
    B,
    C
}

public class CustomerObject : MonoBehaviour
{
    public CustomerStat stat;

    public CustomerState currentState;

    public HairStyle preferredHairStyle;

    Vector3 target;
    int targetIndex;

    [HideInInspector] public int seatIndex;

    float patienceTimer;
    float waitTimer;

    CustomerManager manager;
    CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    public void CreateCustomer(CustomerManager cm)
    {
        manager = cm;
        currentState = CustomerState.WalkingReception;

        patienceTimer = stat.waitingPatienceTime;
    }

    // Update is called once per frame
    void Update()
    {


        switch (currentState)
        {
            case CustomerState.WalkingReception:
                if (Walking(manager.waitingMovementLocations))
                {
                    manager.AddWaitingCustomer(this);
                    currentState = CustomerState.WaitReception;

                    targetIndex = 0;
                }
                break;
            case CustomerState.WaitReception:
                patienceTimer -= Time.deltaTime;
                if (patienceTimer < 0)
                {
                    LostAllPatience();
                }

                SimpleWalk(manager.WaitingLineSpot(this));
                break;
            case CustomerState.WalkingSeat:
                if (SimpleWalk(manager.seatingLocations[seatIndex]))
                {
                    currentState = CustomerState.Seated;
                    patienceTimer = stat.seatedPatienceTime;
                }
                break;
            case CustomerState.Seated:
                patienceTimer -= Time.deltaTime;
                if (patienceTimer < 0)
                {
                    LostAllPatience();
                }

                //Debug Salon
                if (Input.GetKeyDown(KeyCode.V))
                {
                    FinishSalon();
                }

                break;
            case CustomerState.Finished:
                waitTimer -= Time.deltaTime;
                if (waitTimer < 0)
                {
                    currentState = CustomerState.Leaving;
                }
                break;
            case CustomerState.Leaving:
                if (Walking(manager.leavingMovementLocations))
                {
                    targetIndex = 0;
                    DespawnCustomer();
                }
                break;
        }
    }

    public void LostAllPatience()
    {
        currentState = CustomerState.Leaving;
    }

    public void SeatCustomer(int index)
    {
        targetIndex = 0;
        seatIndex = index;
        currentState = CustomerState.WalkingSeat;
    }

    public void FinishSalon()
    {
        targetIndex = 0;
        manager.RemoveSeat(this);
        waitTimer = stat.finishWaitingTime;
        currentState = CustomerState.Finished;
    }

    public void DespawnCustomer()
    {
        manager.DespawnCustomer(this);
        Destroy(gameObject);
    }

    public bool SimpleWalk(Transform pos)
    {
        Vector3 dif = pos.position - transform.position;

        if (dif.magnitude > stat.minDistFromTarget)
        {
            controller.Move(dif.normalized * stat.speed * Time.deltaTime);
        }
        return dif.magnitude <= stat.minDistFromTarget;
    }

    public bool Walking(Transform[] pos)
    {
        if (targetIndex >= pos.Length)
        {
            return true;
        }
        Vector3 dif = pos[targetIndex].position - transform.position;

        if (dif.magnitude <= stat.minDistFromTarget)
        {
            targetIndex++;
            if (targetIndex >= pos.Length)
            {
                return true;
            }
        }

        controller.Move(dif.normalized * stat.speed * Time.deltaTime);

        return false;
    }
}
