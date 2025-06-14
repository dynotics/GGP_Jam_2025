using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Image patienceBar;
    public GameObject patienceBarParent;

    Vector3 target;
    int targetIndex;

    [HideInInspector] public int seatIndex;

    float patienceTimer;
    float waitTimer;

    CustomerManager manager;
    CharacterController controller;

    public HairCycler hairCycler;
    private MirrorSwap assignedStation;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (hairCycler == null)
        {
            GameObject hairHandler = GameObject.FindWithTag("HairCycler");
            if (hairHandler != null && hairHandler.transform.IsChildOf(this.transform)) // only grab it if the tagged object is a child of this client (avoids grabbing another client's hair)
            {
                hairCycler = hairHandler.GetComponent<HairCycler>();
            }
        }
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
                    patienceBarParent.SetActive(true);
                }
                break;
            case CustomerState.WaitReception:
                patienceTimer -= Time.deltaTime;
                patienceBar.fillAmount = Mathf.Clamp01(patienceTimer / stat.waitingPatienceTime); 
                if (patienceTimer < 0)
                {
                    LostAllPatience();
                }

                SimpleWalk(manager.WaitingLineSpot(this));
                break;
            case CustomerState.WalkingSeat:
                if (SimpleWalk(manager.seatingLocations[seatIndex].transform))
                {
                    currentState = CustomerState.Seated;
                    patienceTimer = stat.seatedPatienceTime;

                    patienceBarParent.SetActive(true);
                }
                break;
            case CustomerState.Seated:
                patienceTimer -= Time.deltaTime;
                patienceBar.fillAmount = Mathf.Clamp01(patienceTimer / stat.seatedPatienceTime);
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

        patienceBarParent.SetActive(false);

        CustomerManager cm = manager;
        assignedStation = cm.seatingLocations[index]; // assign specific customer seating loc

        if (hairCycler != null && assignedStation != null)
        {
            hairCycler.trigger = assignedStation.trigger; // assign this station's specific trigger zone to the client's specific hairCycler.
                                                          // allows for multiple stations with multiple customers
            hairCycler.scissorsRect = GameObject.FindWithTag("Scissors").GetComponent<RectTransform>();
        }
    }

    public void FinishSalon()
    {
        targetIndex = 0;
        manager.RemoveSeat(this);
        waitTimer = stat.finishWaitingTime;
        currentState = CustomerState.Finished;

        patienceBarParent.SetActive(false);
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
