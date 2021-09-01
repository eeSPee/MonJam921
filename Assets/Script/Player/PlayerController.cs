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

    public int playerID = 0;
    public float SpawnTime = 0;

    public bool defeated = false;
    public Vector3 input = Vector3.zero;
    public int recordstate = 0;
    public List<Vector4> History = new List<Vector4>();

    protected SpriteRenderer Display;
    protected Collider2D Collision;
    public float last_jump =-1;

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
            History.Add(new Vector4(newinput.x, newinput.y, newinput.z, Time.time - SpawnTime));
        }
        input = newinput;
    }
    private void FixedUpdate()
    {
        HandleControls();
    }
    public void HandleControls()
    {
        if (IsGrounded())
        {
            Vector2 velocity = new Vector2(input.x * hSpeed, rigidbody.velocity.y);
            rigidbody.velocity = velocity;

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
        History.Add(new Vector4(input.x, input.y, input.z, Time.time - SpawnTime));
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
                History.Add(new Vector4(0, 0, 0, Time.time - SpawnTime));
            }

            registermovement = true;
        }
        interactable = null;
        base.TimeReset();
    }

    public void RewriteHistory()
    {
        SpawnTime = Time.time;
        History.Clear();
        History.Add(new Vector4(input.x, input.y, input.z, 0));
    }
}