using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    private Image image;
    private RectTransform slotRect;

    private GameObject currentItemOnSlot;
    [SerializeField] private string[] acceptedTags = { "Scissors", "Hairspray" };

    private void Awake()
    {
        image = GetComponent<Image>();
        slotRect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        foreach (string tag in acceptedTags)
        {
            GameObject tool = GameObject.FindGameObjectWithTag(tag);

            if (tool != null)
            {
                RectTransform toolRect = tool.GetComponent<RectTransform>();
                bool isOverlap = RectTransformUtility.RectangleContainsScreenPoint(slotRect, toolRect.position, null);

                if (RectTransformUtility.RectangleContainsScreenPoint(slotRect, toolRect.position, null))
                {
                    image.enabled = false;
                    currentItemOnSlot = tool;
                    return;
                }
            }

        }
        image.enabled = true;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            string tag = eventData.pointerDrag.tag;

            foreach (string curTag in acceptedTags)
            {
                if (tag == curTag)
                {
                    eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = slotRect.anchoredPosition;
                    image.enabled = false;
                    currentItemOnSlot = eventData.pointerDrag;
                    return;

                }
            }
        }
    }

    private void Update()
    {
        if (currentItemOnSlot != null)
        {
            RectTransform itemRect = currentItemOnSlot.GetComponent<RectTransform>();

            if (!RectTransformUtility.RectangleContainsScreenPoint(slotRect, itemRect.position, null))
            {
                image.enabled = true;
                currentItemOnSlot = null;
            }
        }
    }
}
