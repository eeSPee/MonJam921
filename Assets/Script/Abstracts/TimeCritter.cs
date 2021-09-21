using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeCritter : TimeEntity
{
    public bool Start_Facing = true;
    public float Width = 1f;
    public float Height = 2f;
    public float last_grounded = -1;
    public bool FaceRight = true;
    public override void TimeImprint()
    {
        base.TimeImprint();
        Start_Facing = FaceRight;
    }
    public bool IsGrounded()
    {
        return last_grounded > Time.time;
    }
    public virtual void FaceDirection(bool right)
    {
        FaceRight = right;
        GetComponent<SpriteRenderer>().flipX = !FaceRight;
    }
    public override void TimeReset()
    {
        base.TimeReset();
        lastSafePosition = Start_Position;
        last_grounded = -1;
        FaceDirection(Start_Facing);
    }
    public Vector3 lastSafePosition = Vector3.zero;
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (transform.position.y - Height * .33f > contact.point.y)
            {
                last_grounded = Time.time + .1f;
                if (transform.position.y - Height * .49f > contact.point.y && collision.gameObject.tag == "Ground")
                {
                    lastSafePosition = transform.position;
                }
                break;
            }
        }
    }
    public void ReturnToSafePosition()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = lastSafePosition;
    }
}
