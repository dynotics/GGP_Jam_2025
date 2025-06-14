using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{

    public Transform customerSpawnLocation;

    public Transform[] waitingMovementLocations;
    public Transform[] leavingMovementLocations;

    public GameObject receptionGameObject;
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
        
    }

    public void CreateCustomer()
    {
        CustomerObject customer = Instantiate(customerPrefab, customerSpawnLocation).GetComponent<CustomerObject>();

        currentCustomers.Add(customer);

        customer.CreateCustomer(this);
    }

    public void SeatCustomer()
    {
        if (RequestSeat(currentWaitingCustomers[0]))
        {

        }
        else
        {

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
