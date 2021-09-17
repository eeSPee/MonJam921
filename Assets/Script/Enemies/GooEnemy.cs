using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GooEnemy : TimeEnemy
{
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
