using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GooEnemy : TimeEnemy
{
    //--------------------
    //  GOO ENEMY
    //--------------------
    //This script controlls goo enemies that slow the player down

    //  VARS
    //  SlowTime - the duration of the slow
    //  FUNCTIONS
    //  Awake - declare components, and makes animator animate attack animation depending on the duration of the slow
    //  OnTriggerStay2D - constantly slow the player down to SlowTime seconds, whenever it touches our trigger, and pause this enemy to keep it from moving away

    public float SlowTime = 1;
    protected override void Awake()
    {
        base.Awake();
        animator.SetFloat("Impale_time", SlowTime);
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null && !player.IsSlowed())
            {
                Pause(SlowTime);
                player.Slow(SlowTime);
                animator.SetTrigger("Impale");
            }
        }
    }
}
