using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWinch : TimeInput
{

    //--------------------
    //  TIME WINCH
    //--------------------
    //This script controlls winches (unused)

    //  VARS
    //  Progress - tracks the rotation
    //  requiredProgress - max progress
    //  FUNCTIONS
    //  PlayerInteract - rotates the winch when the player interacts
    //  ChangeState - used to set progress to max/0 when this resets

    public float requiredProgress = 1f;
    float progress = 0;
    public override void PlayerInteract(PlayerController player)
    {
        if (!state )
        {
            progress += Time.deltaTime;
            transform.rotation = Quaternion.Euler(0,0,-360 * progress);
            if (progress >= requiredProgress)
            {
                base.ChangeState(true);
            }
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        progress = value ? requiredProgress : 0;        
    }
}
