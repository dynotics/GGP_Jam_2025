using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCheck : MonoBehaviour
{
    public GameObject Player;
    public Transform cameraPosition;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        cameraPosition = Player.transform.Find("CameraPos");
        transform.position = cameraPosition.position;
    }
}
