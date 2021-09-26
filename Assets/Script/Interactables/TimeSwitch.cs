using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitch : TimeInput
{
    float last_interact_time = -1;

    public AudioSource AudioSourceSwitch;
    public AudioClip AudioClipSwitch;
    public override void PlayerInteract(PlayerController player)
    {
        if (last_interact_time < Time.time)
        {
            base.PlayerInteract(player);
            player.EndInteraction(this);
            last_interact_time = Time.time + .33f;
            AudioSourceSwitch.PlayOneShot(AudioClipSwitch);
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        //GetComponent<SpriteRenderer>().color = value ? Color.green : Color.red;
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
