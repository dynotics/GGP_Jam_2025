using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class NPCInteraction : MonoBehaviour
{
    public TMP_Text indicatorText;

    public CustomerManager manager;
    public GameObject interactPromptUI;

    bool isInteractable;

    private void Start()
    {
        //interactPromptUI.SetActive(false);
    }

    private void Update()
    {
        indicatorText.gameObject.SetActive(manager.currentWaitingCustomers.Count > 0 && isInteractable);
        indicatorText.text = manager.AnyEmptySeats() ? "Press E to Seat a customer!" : "All salons are full";

        if (Input.GetKeyDown(KeyCode.E) && isInteractable)
        {
            manager.SeatCustomer();
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
        }
    }
}
