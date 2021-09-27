using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForceSwitch : TimeSwitch
{
    public float durationForce = 0;
    public ConstantForce2D[] connectedForces = new  ConstantForce2D[0];
    public override void PlayerInteract(PlayerController player)
    {
        if (state)
            return;
        base.PlayerInteract(player);
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        if (connectedForces.Length>0)
        {
            if (state)
            {
                StartCoroutine(FireForce());
            }
            else
            {
                foreach (ConstantForce2D connectedForce in connectedForces)
                {
                    connectedForce.enabled = false;
                }
            }
        }
    }
    IEnumerator FireForce()
    {
        foreach (ConstantForce2D connectedForce in connectedForces)
        {
            connectedForce.enabled = true;
        }
        yield return new WaitForSeconds(durationForce);
        foreach (ConstantForce2D connectedForce in connectedForces)
        {
            connectedForce.enabled = false;
        }
    }
    public override void TimeReset()
    {
        StopAllCoroutines();
        base.TimeReset();
        foreach (ConstantForce2D connectedForce in connectedForces)
        {
            connectedForce.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        }
    }
}
