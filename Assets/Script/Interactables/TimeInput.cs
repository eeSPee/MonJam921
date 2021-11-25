using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInput : TimeInteractable
{
    //--------------------
    //  TIME INPUT
    //--------------------
    //This script controlls interactables that the player can itneract with, like pickups

    //  FUNCTIONS
    //  OnTriggerEnter2D - begins interaction with this object
    //  OnTriggerExit2D - ends interaction with this object

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().StartInteraction(this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().EndInteraction(this);
        }
    }
}
