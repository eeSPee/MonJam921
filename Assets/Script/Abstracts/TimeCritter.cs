using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeCritter : TimeEntity
{
    //--------------------
    //  TIME CRITTER
    //--------------------
    //Abstract class where all the mobs derive from

    //  VARS
    //  Start_Facing - Starting facing
    //  FaceRight - Facing direction
    //  Width, Height - Dimensions of this entity
    //  last_grounded - Last time it touched ground
    //  lastSafePosition - Last safe position to reset to
    //  FUNCTIONS
    //  TimeImprint, TimeReset - see Time Entity
    //  IsGrounded - did it touch ground recently
    //  FaceDirection - change facing
    //  IsGrounded - assigns a listener that changes the scene to our desired scene
    //  OnCollisionStay2D - checks if ground is safe, and stores last safe position
    //  ReturnToSafePosition - resets to stored last safe position

    #region Facing
    public bool Start_Facing = true;
    public bool FaceRight = true;
        public virtual void FaceDirection(bool right)
    {
        FaceRight = right;
        GetComponent<SpriteRenderer>().flipX = !FaceRight;
    }
    #endregion
    #region Time
    public override void TimeImprint()
    {
        base.TimeImprint();
        Start_Facing = FaceRight;
    }
    public override void TimeReset()
    {
        base.TimeReset();
        lastSafePosition = Start_Position;
        last_grounded = -1;
        FaceDirection(Start_Facing);
    }
    #endregion
    #region Ground and collision
    public float Width = 1f;
    public float Height = 2f;
    public float last_grounded = -1;
    public bool IsGrounded()
    {
        return last_grounded > Time.time;
    }
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
    #endregion
    #region Safe Position
    public Vector3 lastSafePosition = Vector3.zero;
    public void ReturnToSafePosition()
    {
        rigidbody.velocity = Vector2.zero;
        transform.position = lastSafePosition;
    }
    #endregion
}
