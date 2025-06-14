using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    public Transform customerSpawnLocation;

    public Transform[] waitingMovementLocations;
    public Transform[] leavingMovementLocations;

    public Transform[] waitingLocations;
    public Transform[] seatingLocations;

    CustomerObject[] currentSeatedCustomers;
    

    [Space(10)]
    public GameObject customerPrefab;

    List<CustomerObject> currentCustomers;
    List<CustomerObject> currentWaitingCustomers;

    // Start is called before the first frame update
    void Start()
    {
        currentSeatedCustomers = new CustomerObject[seatingLocations.Length];
    }

    // Update is called once per frame
    void Update()
    {
        //Debug Salon
        if (Input.GetKeyDown(KeyCode.T))
        {
            SeatCustomer();
        }
    }

    public void CreateCustomer()
    {
        CustomerObject customer = Instantiate(customerPrefab, customerSpawnLocation).GetComponent<CustomerObject>();

        currentCustomers.Add(customer);

        customer.CreateCustomer(this);
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
                return waitingLocations[Mathf.Min(i, waitingLocations.Length)];
            }
        }

        return null;
    }

    public void SeatCustomer()
    {
        if (RequestSeat(currentWaitingCustomers[0]))
        {
            currentWaitingCustomers.RemoveAt(0);
        }
        else
        {
            //If there are no available seats
        }
    }

    public bool RequestSeat(CustomerObject customer)
    {
        for (int i = 0; i < currentSeatedCustomers.Length; i++)
        {
            if (currentSeatedCustomers[i] == null)
            {
                customer.SeatCustomer(i);
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
}
