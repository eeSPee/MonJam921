using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickup : TimeInteractable
{
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
