using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForceSwitch : TimeSwitch
{
    //--------------------
    //  TIME FORCE SWITCH
    //--------------------
    //This script works like a time switch, but triggers a force whenever it is interacted with

    //  VARS
    //  durationForce - duration of the force when activated
    //  connectedForces - connected Force2D components that trigger
    //  FUNCTIONS
    //  PlayerInteract - see TimeSwitch
    //  ChangeState - starts the FireForce coroutine
    //  FireForce - coroutine that enables the connected forces, and after durationForce seconds, disables them
    //  TimeReset - disables all forces and resets the switch

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
