using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HairCycler : MonoBehaviour
{
    public List<GameObject> hairStyles;           
    public RectTransform scissorsRect;            
    public RectTransform trigger;            

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

        currentIndex = (currentIndex + 1) % hairStyles.Count;
        hairStyles[currentIndex].SetActive(true);
    }

    public void TryCycleHair()
    {
        if (!canSwitch || hairStyles.Count == 0)
            return;

        CycleHair();
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

