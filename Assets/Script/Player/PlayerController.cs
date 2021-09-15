using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : TimeCritter
{
    public static PlayerController player;

    bool faceRight = true;
    float hSpeed = 6;
    float airSpeed = 6;
    float JumpSpeed = 500;
    public bool registermovement = false;
    protected float delay = 0;

    public int playerID = 0;
    public float SpawnTime = 0;

    public bool defeated = false;
    public Vector3 input = Vector3.zero;
    public int recordstate = 0;
    public List<Vector4> History = new List<Vector4>();

    protected SpriteRenderer Display;
    protected Collider2D Collision;
    public float last_jump = -1;
    public float last_interact = -1;
    float slowedTime = 0;

    public TimeInteractable interactable;
    protected override void Awake()
    {
        Display = GetComponent<SpriteRenderer>();
        Collision = GetComponent<Collider2D>();
        base.Awake();
        RewriteHistory();
        registermovement = true;
        recordstate = 1;
        SpawnTime = Time.time;
    }

    protected virtual void HandleInput()
    {
        if (!registermovement)
            return;

        Vector3 newinput = new Vector3(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical"),
            Input.GetAxisRaw("Submit")
            );

        if (newinput.x != input.x || newinput.y != input.y || newinput.z != input.z)
        {
            History.Add(new Vector4(newinput.x, newinput.y, newinput.z, Time.time - SpawnTime + delay));
        }
        input = newinput;
    }
    private void FixedUpdate()
    {
        HandleInput();
        HandleMovement();
        CarryPickup();
        UpdateAnimator();
    }
    public float GetMovementSpeed()
    {
        return hSpeed * (IsSlowed() ? .33f : 1f);
    }
    public void HandleMovement()
    {
        if (IsGrounded())
        {

            faceRight = (input.x == 0) ? faceRight : (input.x>0);
            rigidbody.velocity = new Vector2(input.x * GetMovementSpeed(), rigidbody.velocity.y);

            if (input.y > 0)
            {
                if (last_jump < Time.time)
                {
                    last_jump = Time.time + .1f;
                    rigidbody.AddForce(transform.up * JumpSpeed * rigidbody.mass);
                }
            }
            if (input.z != 0 && last_interact < Time.time)
            {
                if (myPickup != null)
                {
                    DropItem(true);
                }
                else if (interactable != null)
                {
                    interactable.PlayerInteract(this);
                }
            }
        }
        else
        {
            rigidbody.velocity += (Vector2)input * airSpeed * Time.deltaTime;
            if (input.z != 0)
            {
                if (last_interact < Time.time)
                {
                    last_interact = Time.time + .3f;
                    if (myPickup != null)
                    {
                        DropItem(false);
                    }
                }
            }
        }
    }
    public void UpdateAnimator()
    {
        GetComponent<SpriteRenderer>().flipX = !faceRight;

        bool grounded = last_grounded+.05f > Time.time;
        if (input.y>0 && animator.GetBool("grounded"))
        {
            animator.SetTrigger("Jump");
        }
        animator.SetBool("grounded", grounded);
        if (grounded)
        {
            animator.SetBool("Walking", input.x!=0);
        }
        else
        {
            animator.SetFloat("ySpeed", rigidbody.velocity.y);
        }
    }

    public virtual void Kill()
    {
        if (!registermovement)
        {
            return;
        }
        defeated = true;
        enabled = false;
        registermovement = false;
        History.Add(new Vector4(input.x, input.y, input.z, Time.time - SpawnTime+ delay));
        History.Add(new Vector4(0, 0, 0, -1));
        Explode();
    }
    public void Explode()
    {
        GameObject poof = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/Explosion"));
        poof.transform.position = transform.position;
    }
    public override void TimeReset()
    {
        last_jump = -1;
        last_interact = -1;
        if (!defeated && registermovement)
        {
            if (recordstate == 1)
            {
                History.Add(new Vector4(0, 0, 0, Time.time - SpawnTime+ delay));
            }

            registermovement = true;
        }
        delay = 0;
        slowedTime = 0;
        interactable = null;
        myPickup = null;
        base.TimeReset();
    }

    public void RewriteHistory()
    {
        SpawnTime = Time.time;
        History.Clear();
        History.Add(Vector4.zero);
    }
    public void Slow(float dur)
    {
        slowedTime = Time.time + dur;
    }
    public bool IsSlowed()
    {
        return slowedTime > Time.time;
    }
    public void KnockBack(float x, float y)
    {
        last_grounded = 0;
        rigidbody.position += Vector2.up * .1f;
        rigidbody.velocity = new Vector2(x,y);
    }
    public virtual void Delay(float t)
    {
        delay += t;
        slowedTime -= t;
        Debug.Log("Delay " + name + " by " + t + " seconds");
    }
    public float GetDelay()
    {
        return delay;
    }
    public virtual bool IsOriginal()
    {
        return true;
    }
    TimeDroppable myPickup;
    public void PickUpItem(TimeDroppable pickup)
    {
        if (pickup == null)
            return;
        DropItem(false);
        myPickup = pickup;
        myPickup.ChangeState(false);
        last_interact = Time.time + .3f;
    }
    public void DropItem(bool thrown)
    {
        if (myPickup != null)
        {
            myPickup.ChangeState(true);
            if (thrown)
            {
                myPickup.rigidbody.velocity = new Vector2(3 * (faceRight ? 1 : -1), 5);
            }
            myPickup = null;
            last_interact = Time.time + .3f;
        }
    }
    public void CarryPickup()
    {
        if (myPickup != null)
        {
            myPickup.transform.position = new Vector3(transform.position.x, transform.position.y, myPickup.transform.position.z);
        }
    }
}