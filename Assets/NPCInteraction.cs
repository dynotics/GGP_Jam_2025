using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public Character1Movement characterScript;
    public GameObject interactPromptUI;
    private bool isInteractable = false;

    private void Start()
    {
        interactPromptUI.SetActive(false);
    }

    private void Update()
    {
        if (isInteractable && Input.GetKeyDown(KeyCode.E))
        {
            characterScript.BeSeated();
            interactPromptUI.SetActive(false);
            isInteractable = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (characterScript != null && characterScript.waitReception)
            {
                isInteractable = true;
                interactPromptUI.SetActive(true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isInteractable = false;
            interactPromptUI.SetActive(false);
        }
    }
}
