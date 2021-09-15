using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitch : TimeInput
{
    float last_interact_time = -1;
    public override void PlayerInteract(PlayerController player)
    {
        if (last_interact_time < Time.time)
        {
            base.PlayerInteract(player);
            last_interact_time = Time.time + .33f;
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        //GetComponent<SpriteRenderer>().color = value ? Color.green : Color.red;
        animator.SetBool("enabled", value);
    }
    public override void TimeReset()
    {
        base.TimeReset();
        last_interact_time = -1;
    }
}
