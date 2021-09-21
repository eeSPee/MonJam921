using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForceSwitch : TimeSwitch
{
    public AudioSource AudioSourceSwitch;
    public AudioClip AudioClipSwitch;
    public float durationForce = 0;
    public ConstantForce2D connectedForce;

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
