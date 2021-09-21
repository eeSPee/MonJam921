using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantDeathTrigger : MonoBehaviour
{
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
