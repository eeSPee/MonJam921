using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeCritter : TimeEntity
{
    public bool OriginalFacing = true;
    public float Width = 1f;
    public float Height = 2f;
    public float last_grounded = -1;
    public bool FaceRight = true;
    public override void TimeImprint()
    {
        base.TimeImprint();
        OriginalFacing = FaceRight;
    }
    public bool IsGrounded()
    {
        return last_grounded > Time.time;
    }
    public void FaceDirection(bool right)
    {
        FaceRight = right;
        GetComponent<SpriteRenderer>().flipX = !FaceRight;
    }
    public override void TimeReset()
    {
        base.TimeReset();
        last_grounded = -1;
        FaceDirection(OriginalFacing);
    }
    public virtual void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (transform.position.y - Height * .25f > contact.point.y)
            {
                last_grounded = Time.time + .1f;
                break;
            }
        }
    }
}
