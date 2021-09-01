using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeWinch : TimeInput
{
    public float requiredProgress = 1f;
    float progress = 0;
    public override void PlayerInteract()
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
