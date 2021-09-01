using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : TimeCritter
{
    public bool FaceRight = true;
    bool OriginalFacing = true;
    bool Walking = true;
    public override void TimeImprint()
    {
        base.TimeImprint();
        OriginalFacing = FaceRight;
    }
    public override void TimeReset()
    {
        UnDie();
        base.TimeReset();
        FaceRight = OriginalFacing;
        Walking = true; 
        StopAllCoroutines();
    }
    private void Update()
    {
        if (Walking && IsGrounded())
        {
            rigidbody.velocity = new Vector2(((FaceRight) ? 1 : -1) , rigidbody.velocity.y);
        }
    }
    public override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].point.y < transform.position.y - .45f * transform.up.y)
        {
            last_grounded = Time.time + .1f;
        }
        else
        {
            if ((collision.contacts[0].point.x > transform.position.x) == FaceRight)
            {
                FaceRight = !FaceRight;
            }
            
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Interactable" && collision.contacts[0].normalImpulse > 7f)
        {
            Die();
        }
    }
    public void Die()
    {
        gameObject.SetActive(false);
    }
    public void UnDie()
    {
        gameObject.SetActive(true);
    }
}
