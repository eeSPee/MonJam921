using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitch : TimeInput
{
    //--------------------
    //  TIME SWITCH
    //--------------------
    //This script controlls switches and reset

    //  VARS
    //  AudioSourceApple - audiosource on this object
    //  AudioClipSwitch - audioclips that play when interacted
    //  SingleUse - can be reused?
    //  last_interact_time - prevents the player for interacting too often and bugging the animation
    //  FUNCTIONS
    //  PlayerInteract - when the player interacts, triggers the animation, plays the sound and changes state
    //  ChangeState - Change state and animation
    //  TimeReset - Resets the state and itneract time
    //  UpdateListener - updates listener volume based on distance


    public bool SingleUse = false;
    float last_interact_time = -1;

    public AudioSource AudioSourceSwitch;
    public AudioClip AudioClipSwitch;
    public override void PlayerInteract(PlayerController player)
    {
        if (SingleUse && state)
            return;
        if (last_interact_time < Time.time)
        {
            base.PlayerInteract(player);
            if (SingleUse)
            {
                player.EndInteraction(this);
            }
            last_interact_time = Time.time + .33f;
            AudioSourceSwitch.PlayOneShot(AudioClipSwitch);
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        animator.SetBool("enabled", value);
    }
    public override void TimeReset()
    {
        base.TimeReset();
        last_interact_time = -1;
    }

    private void Update()
    {
        UpdateListener();
    }

    public void UpdateListener()
    {
		float ListenerDistance = ((Vector2)transform.position - (Vector2)PlayerController.player.transform.position).magnitude;
		AudioSourceSwitch.volume = Mathf.Clamp(1 - ((ListenerDistance-GameController.MinListenerDistance) / (GameController.MaxListenerDistance - GameController.MinListenerDistance)),0,1);
    } 
}
