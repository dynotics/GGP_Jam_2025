using UnityEngine;

public class HaircutTrigger : MonoBehaviour
{
    public MirrorSwap myStation; // for each individual station :)

    void Update()
    {
        RectTransform scissors = GameObject.FindWithTag("Scissors")?.GetComponent<RectTransform>();
        if (scissors == null || myStation == null || myStation.npc == null)
            return; 

        if (RectTransformUtility.RectangleContainsScreenPoint(GetComponent<RectTransform>(), scissors.position, null)) // are scissors hovering over zone?
        {
            CustomerObject customer = myStation.npc;
            if (customer.currentState == CustomerState.Seated && customer.hairCycler != null) // only if seated and hairCycler attached
            {
                customer.hairCycler.TryCycleHair();
            }
        }
    }
}

