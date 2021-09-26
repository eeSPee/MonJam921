using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitch : TimeInput
{
    float last_interact_time = -1;

    public float MinListenerDistance = 1;
    public float MaxListenerDistance = 25;

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
        float ListenerDistance = Vector3.Distance(transform.position, PlayerController.player.transform.position);

        if (ListenerDistance <= MinListenerDistance)
        {
            AudioSourceSwitch.volume = 1;
        }
        else if (ListenerDistance > MaxListenerDistance)
        {
            AudioSourceSwitch.volume = 0;
        }
        else
        {
            AudioSourceSwitch.volume = 1 - ((ListenerDistance - MinListenerDistance) / (MaxListenerDistance - MinListenerDistance));
        }
    }
}
