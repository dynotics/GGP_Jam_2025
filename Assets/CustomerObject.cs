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

public class CustomerObject : MonoBehaviour
{
    public CustomerStat stat;

    public CustomerState currentState;

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
                Walking(manager.waitingMovementLocations);
                break;
            case CustomerState.WaitReception:
                patienceTimer -= Time.deltaTime;
                //SimpleWalk();
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
                break;
            case CustomerState.Finished:
                waitTimer -= Time.deltaTime;
                if (waitTimer < 0)
                {
                    currentState = CustomerState.Leaving;
                }
                break;
            case CustomerState.Leaving:
                Walking(manager.waitingMovementLocations);
                break;
        }
    }

    public void LostAllPatience()
    {
        currentState = CustomerState.Leaving;
    }

    public void SeatCustomer(int index)
    {
        seatIndex = index;
        currentState = CustomerState.WalkingSeat;
    }

    public void FinishSalon()
    {
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
        Vector3 dif = transform.position - pos.position;
        controller.Move(dif.normalized * stat.speed * Time.deltaTime);
        return dif.magnitude <= stat.minDistFromTarget;
    }

    public bool Walking(Transform[] pos)
    {
        if (targetIndex >= pos.Length)
        {
            return true;
        }
        Vector3 dif = transform.position - pos[targetIndex].position;

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
