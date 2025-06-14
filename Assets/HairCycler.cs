using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairCycler : MonoBehaviour
{
    public List<GameObject> hairStyles;           
    public RectTransform scissorsRect;            
    public RectTransform triggerZone;            

    private int currentIndex = 0;
    private bool canSwitch = true;

    private void Start()
    {
        if (scissorsRect == null)
            scissorsRect = GameObject.FindWithTag("Scissors")?.GetComponent<RectTransform>();

        if (triggerZone == null)
            triggerZone = GameObject.FindWithTag("HaircutTrigger")?.GetComponent<RectTransform>();

        foreach (GameObject hair in hairStyles)
        {
            hair.SetActive(false);
        }
    }
    private void Update()
    {
        if (RectOverlaps(scissorsRect, triggerZone) && canSwitch)
        {
            CycleHair();
            StartCoroutine(SwitchCooldown());
        }
    }

    void CycleHair()
    {
        hairStyles[currentIndex].SetActive(false);
        currentIndex = (currentIndex + 1) % hairStyles.Count;
        hairStyles[currentIndex].SetActive(true);
    }

    IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(0.5f); // prevent rapid-fire switching
        canSwitch = true;
    }

    bool RectOverlaps(RectTransform a, RectTransform b)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(b, a.position, null);
    }
}

