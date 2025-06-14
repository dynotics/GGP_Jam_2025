using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Custom/CustomerStat")]
public class CustomerStat : ScriptableObject
{
    public float speed;
    public float minDistFromTarget;

    public float waitingPatienceTime;
    public float seatedPatienceTime;

    public float finishWaitingTime;

    public string[] finishText;
    public string[] failText;
}
