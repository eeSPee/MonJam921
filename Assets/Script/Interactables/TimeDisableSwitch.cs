using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDisableSwitch : TimeSwitch
{
    public bool TwoSide = false;
    public GameObject[] connectedGameObject = new GameObject[0];
    public override void PlayerInteract(PlayerController player)
    {
        if (!TwoSide && state)
            return;
        base.PlayerInteract(player);
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        foreach (GameObject connectedGameObject in connectedGameObject)
        {
            connectedGameObject?.SetActive(!connectedGameObject.activeInHierarchy);
        }
    }

}
