using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForceWinch : TimeWinch
{
    //--------------------
    //  TIME FORCE WINCH
    //--------------------
    //This script works like a time winch, but triggers a force whenever it is interacted with

    //  VARS
    //  connectedForce - connected Force2D component
    //  FUNCTIONS
    //  PlayerInteract - fires the connectedForce every frame

    public ConstantForce2D connectedForce;
    public override void PlayerInteract(PlayerController player)
    {
        base.PlayerInteract(player);
        if (!state && connectedForce!=null)
        {
            connectedForce.GetComponent<Rigidbody2D>().AddForce(connectedForce.force);
            connectedForce.GetComponent<Rigidbody2D>().AddRelativeForce(connectedForce.relativeForce);
            connectedForce.GetComponent<Rigidbody2D>().AddTorque(connectedForce.torque);
        }
    }
}
