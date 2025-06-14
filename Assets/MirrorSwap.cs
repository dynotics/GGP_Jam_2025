using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorSwap : MonoBehaviour
{
    public PlayerMovement player;
    public CustomerObject npc;
    public Camera defaultCam;
    public Camera mirrorCam;
    public RectTransform trigger;

    private void Start()
    {
        if (mirrorCam!= null)
        {
            mirrorCam.enabled = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.tag == "Player";
        bool npcSeated = npc != null && npc.currentState == CustomerState.Seated;

        if (isPlayer && npcSeated)
        {
            if (defaultCam != null)
            {
                defaultCam.enabled = false;
                player.isFrozen = false;
            }

            if (mirrorCam != null)
            {
                mirrorCam.enabled = true;
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                player.isFrozen = true;

            }
        }
    }
    
}
