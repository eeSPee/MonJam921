using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimePickup : TimeInteractable
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ChangeState(false);
        }
    }
    public override void ChangeState(bool value)
    {
        base.ChangeState(value);
        gameObject.SetActive(state);
    }
}
