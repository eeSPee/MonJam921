using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDisableSwitch : TimeSwitch
{
    public bool Reverse = false;
    public GameObject connectedGameObject;
    public override void PlayerInteract(PlayerController player)
    {
        if (state)
            return;
        base.PlayerInteract(player);
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        if (connectedGameObject != null)
        {
            connectedGameObject.SetActive(Reverse ? !value : value);
        }
    }

}
