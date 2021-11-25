using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDisableSwitch : TimeSwitch
{
    //--------------------
    //  TIMEDISABLE SWITCH
    //--------------------
    //This script controlls a time switch that changes the disable state of connected gameobjects when used

    //  VARS
    //  connectedGameObject - game objects connected
    //  FUNCTIONS
    //  ChangeState - also changes state of connected gameobjects

    public GameObject[] connectedGameObject = new GameObject[0];
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        foreach (GameObject connectedGameObject in connectedGameObject)
        {
            connectedGameObject?.SetActive(!connectedGameObject.activeInHierarchy);
        }
    }

}
