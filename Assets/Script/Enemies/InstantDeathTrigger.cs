using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathTrigger : MonoBehaviour
{
    //--------------------
    //  DEATH TRIGGER
    //--------------------
    //This script resets the player when the trigger is touched, and penalizes the player

    //  VARS
    //  DelayPenalty - how much time it costs for the player
    //  FUNCTIONS
    //  OnTriggerStay2D - resets the player to the latest safe position, and gives a time penalty

    public float DelayPenalty = 1;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player!=null)
            {
                player.Delay(DelayPenalty);
                player.ReturnToSafePosition();
            }
        }
    }
}
