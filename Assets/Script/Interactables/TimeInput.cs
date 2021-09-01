using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInput : TimeInteractable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().interactable = this;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.interactable == this)
            {
                player.interactable = null;
            }
        }
    }
}
