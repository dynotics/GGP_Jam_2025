using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    public Transform customerSpawnLocation;

    public Transform[] waitingMovementLocations;
    public Transform[] leavingMovementLocations;

    public Transform[] waitingLocations;
    public MirrorSwap[] seatingLocations;

    CustomerObject[] currentSeatedCustomers;
    

    [Space(10)]
    public GameObject customerPrefab;

    List<CustomerObject> currentCustomers;
    [HideInInspector] public List<CustomerObject> currentWaitingCustomers;

    [Space(10)]

    public float timeBetweenBurst;
    public float timeDecay;

    float burstTimer;
    int burstAmount;
    List<int> burstList;

    // Start is called before the first frame update
    void Start()
    {
        currentSeatedCustomers = new CustomerObject[seatingLocations.Length];

        currentCustomers = new List<CustomerObject>();
        currentWaitingCustomers = new List<CustomerObject>();

        CreateNewList();
    }

    // Update is called once per frame
    void Update()
    {

        burstTimer -= Time.deltaTime;
        if (burstTimer < 0)
        {
            int r = Random.Range(0, burstList.Count - 1);
            StartCoroutine(SpawnCustomers(burstList[r]));
            burstList.Remove(r);

            if (burstList.Count >= 0)
            {
                CreateNewList();
            }

            burstTimer = timeBetweenBurst - timeDecay * burstAmount;
            burstAmount++;
        }

        //Debug Salon
        
        /*
        if (Input.GetKeyDown(KeyCode.E))
        {
            SeatCustomer();
        }
        
        if (Input.GetKeyDown(KeyCode.B))
        {
            CreateCustomer();
        }*/
    }

    public IEnumerator SpawnCustomers(int x)
    {
        yield return new WaitForSeconds(.1f);
        for (int i = 0; i < x; i++)
        {
            CreateCustomer();
            yield return new WaitForSeconds(Random.Range(.3f, 1.7f));
        }
    }

    public void CreateCustomer()
    {
        CustomerObject customer = Instantiate(customerPrefab, customerSpawnLocation.position, Quaternion.identity).GetComponent<CustomerObject>();

        currentCustomers.Add(customer);

        customer.CreateCustomer(this);

        Debug.Log("Create Customer");
    }

    public void AddWaitingCustomer(CustomerObject customer)
    {
        currentWaitingCustomers.Add(customer);
    }

    public Transform WaitingLineSpot(CustomerObject customer)
    {
        for (int i = 0; i < currentWaitingCustomers.Count; i++)
        {
            if (customer == currentWaitingCustomers[i])
            {
                return waitingLocations[Mathf.Min(i, waitingLocations.Length-1)];
            }
        }

        return null;
    }

    public bool SeatCustomer()
    {
        if (currentWaitingCustomers.Count != 0 && RequestSeat(currentWaitingCustomers[0]))
        {
            currentWaitingCustomers.RemoveAt(0);
            return true;
        }
        else
        {
            Debug.Log("No Available seats");
            return false;
            //If there are no available seats
        }
    }

    public bool AnyEmptySeats()
    {
        for (int i = 0; i < currentSeatedCustomers.Length; i++)
        {
            if (currentSeatedCustomers[i] == null)
            {
                return true;
            }
        }
        return false;
    }

    public bool RequestSeat(CustomerObject customer)
    {
        for (int i = 0; i < currentSeatedCustomers.Length; i++)
        {
            if (currentSeatedCustomers[i] == null)
            {
                customer.SeatCustomer(i);
                seatingLocations[i].npc = customer;
                currentSeatedCustomers[i] = customer;
                return true;
            }
        }
        return false;
    }

    public void RemoveSeat(CustomerObject customer)
    {
        for (int i = 0; i < currentSeatedCustomers.Length; i++)
        {
            if (currentSeatedCustomers[i] == customer)
            {
                currentSeatedCustomers[i] = null;
                return;
            }
        }
    }

    public void DespawnCustomer(CustomerObject customer)
    {
        currentCustomers.Remove(customer);

        //Just in case the customer is waiting in the current line
        currentWaitingCustomers.Remove(customer);
    }

    public void CreateNewList()
    {
        burstList = new List<int>();
        for (int i = 0; i < 3; i++)
        {
            burstList.Add(i);
            burstList.Add(1 + i);
        }
        
    }
}
