using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : TimeCritter
{
    public static PlayerController player;

    float hSpeed = 6;
    float airSpeed = 6;
    float JumpSpeed = 500;
    bool registermovement = false;
    float delay = 0;

    public int playerID = 0;
    public float SpawnTime = 0;

    public bool defeated = false;
    public Vector3 input = Vector3.zero;
    public int recordstate = 0;
    public List<Vector4> History = new List<Vector4>();

    protected SpriteRenderer Display;
    protected Collider2D Collision;
    public float last_jump =-1;
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

    protected virtual void Update()
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
        HandleControls();
    }
    public float GetMovementSpeed()
    {
        return hSpeed * (IsSlowed() ? .33f : 1f);
    }
    public void HandleControls()
    {
        if (IsGrounded())
        {
            rigidbody.velocity = new Vector2(input.x * GetMovementSpeed(), rigidbody.velocity.y);

            if (input.y > 0)
            {
                if (last_jump < Time.time)
                {
                    last_jump = Time.time + .1f;
                    rigidbody.AddForce(transform.up * JumpSpeed * rigidbody.mass);
                }
            }
            if (input.z != 0 && interactable!= null)
            {
                interactable.PlayerInteract();
            }
        }
        else
        {
            rigidbody.velocity += (Vector2)input * airSpeed * Time.deltaTime;
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
        if (!defeated)
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
        base.TimeReset();
    }

    public void RewriteHistory()
    {
        SpawnTime = Time.time;
        History.Clear();
        History.Add(new Vector4(input.x, input.y, input.z, 0));
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
}