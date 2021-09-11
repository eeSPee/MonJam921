using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeCritter : TimeEntity
{
    public float last_grounded = -1;
    public bool IsGrounded()
    {
        return last_grounded > Time.time;
    }
    public override void TimeReset()
    {
        base.TimeReset();
        last_grounded = -1;
    }
        public virtual void OnCollisionStay2D(Collision2D collision)
    {
        //if (collision.gameObject.tag == "Ground")
        {
            if (rigidbody.velocity.y<=0 && transform.position.y - .8f * transform.up.y > collision.contacts[0].point.y)
            {
                last_grounded = Time.time + .1f;
            }
        }
    }
}
