using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeForceWinch : TimeWinch
{
    public ConstantForce2D connectedForce;
    public override void PlayerInteract()
    {
        base.PlayerInteract();
        if (!state && connectedForce!=null)
        {
            connectedForce.GetComponent<Rigidbody2D>().AddForce(connectedForce.force);
            connectedForce.GetComponent<Rigidbody2D>().AddRelativeForce(connectedForce.relativeForce);
            connectedForce.GetComponent<Rigidbody2D>().AddTorque(connectedForce.torque);
        }
    }
}
