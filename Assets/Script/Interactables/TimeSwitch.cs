using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeSwitch : TimeInput
{
    float last_interact_time = -1;
    public override void PlayerInteract()
    {
        if (last_interact_time < Time.time)
        {
            base.PlayerInteract();
            last_interact_time = Time.time + .33f;
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        GetComponent<SpriteRenderer>().color = value ? Color.green : Color.red;
    }
    public override void TimeReset()
    {
        base.TimeReset();
        last_interact_time = -1;
    }
}
