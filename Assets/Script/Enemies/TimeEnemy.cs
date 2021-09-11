using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : TimeCritter
{
    public float MoveSpeed = 1f;
    public bool FaceRight = true;
    bool OriginalFacing = true;
    protected bool Walking = true;
    protected float stunTime = 0;
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
        stunTime = 0;
        StopAllCoroutines();
    }
    private void Update()
    {
        Vector2 velocity = rigidbody.velocity;

        if (stunTime > Time.time)
        {
            velocity.x = 0;
        }
        else
        {
            if (rigidbody.IsSleeping())
            {
                rigidbody.WakeUp();
            }
            if (IsGrounded() && Walking)
            {
                velocity.x = ((FaceRight) ? 1 : -1) * MoveSpeed;
            }
        }
        rigidbody.velocity = velocity;
    }
    public void Pause(float pausetime)
    {
        stunTime = Time.time+pausetime;
    }
    public override void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.contacts[0].point.y < transform.position.y - .45f)
        {
            last_grounded = Time.time + .1f;
        }
        else if ((FaceRight && collision.contacts[0].point.x > transform.position.x) || (!FaceRight && collision.contacts[0].point.x < transform.position.x))
        {
            FaceRight = !FaceRight;
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
