using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForceSwitch : TimeSwitch
{
    public float durationForce = 0;
    public ConstantForce2D connectedForce;

    public AudioSource AudioSourceSwitch;
    public AudioClip AudioClipSwitch;

    public float MinListenerDistance=1;
    public float MaxListenerDistance=25;

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
        AudioSourceSwitch.volume = 1 - ((ListenerDistance-MinListenerDistance) / (MaxListenerDistance - MinListenerDistance));
      }
    }

    public override void PlayerInteract(PlayerController player)
    {
        if (state)
            return;
        base.PlayerInteract(player);
        AudioSourceSwitch.PlayOneShot(AudioClipSwitch);
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        if (connectedForce != null)
        {
            if (state)
            {
                StartCoroutine(FireForce());
            }
            else
            {
                connectedForce.enabled = false;
            }
        }
    }
    IEnumerator FireForce()
    {
        connectedForce.enabled = true;
        yield return new WaitForSeconds(durationForce);
        connectedForce.enabled = false;
    }
    public override void TimeReset()
    {
        StopAllCoroutines();
        base.TimeReset();
    }
}
