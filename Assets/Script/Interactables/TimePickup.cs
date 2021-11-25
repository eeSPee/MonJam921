using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickup : TimeInteractable
{
    //--------------------
    //  TIME PICKUP
    //--------------------
    //This script controlls pickups (flies and clocks)

    //  VARS
    //  Important - Is this important to game progression
    //  AudioSource - audio source of this object
    //  AudioClip - clip played on pickup
    //  FUNCTIONS
    //  OnTriggerEnter2D - change state of this object when the player touches it
    //  ChangeState - disable this object, and update the UI

    public bool Important = false;
    public AudioSource AudioSourcePickup;
    public AudioClip AudioClipPickup;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
          AudioSource.PlayClipAtPoint(AudioClipPickup, transform.position,1);
          ChangeState(false);
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        gameObject.SetActive(state);
        UIProgressTracker.main.IncrementProgress(Important);
    }
}
