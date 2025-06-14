using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairCycler : MonoBehaviour
{
    public List<GameObject> hairStyles;           
    public RectTransform scissorsRect;            
    public RectTransform trigger;
    public CustomerObject curCustomer;

    private int currentIndex = 0;
    private bool canSwitch = true;

    private void Start()
    {
        if (scissorsRect == null)
            scissorsRect = GameObject.FindWithTag("Scissors").GetComponent<RectTransform>();


        foreach (GameObject hair in hairStyles)
        {
            hair.SetActive(false);
        }
    }


    public void CycleHair()
    {
        if (currentIndex >= 0)
            hairStyles[currentIndex].SetActive(false);

        currentIndex = (currentIndex + 1) % hairStyles.Count; // always moves fwd by 1, but once reaches 4 no remainder, so loops
        hairStyles[currentIndex].SetActive(true);
    }

    public void TryCycleHair()
    {
        if (!canSwitch || hairStyles.Count == 0)
            return;

        CycleHair();

        if (curCustomer != null && curCustomer.currentState == CustomerState.Seated)
        {
            GameObject curHair = hairStyles[currentIndex];

            if (curCustomer.preferredHairObj == curHair)
            {
                curCustomer.FinishSalon(); // leaves if satisifed!
            }
        }
        
        StartCoroutine(SwitchCooldown());
    }

    IEnumerator SwitchCooldown()
    {
        canSwitch = false;
        yield return new WaitForSeconds(0.75f); // prevent rapid-fire switching
        canSwitch = true;
    }

    bool RectOverlaps(RectTransform a, RectTransform b)
    {
        return RectTransformUtility.RectangleContainsScreenPoint(b, a.position, null);
    }
}

