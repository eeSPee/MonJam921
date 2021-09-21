using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : TimeCritter
{
    public static int randomHat;
    public static PlayerController player;

    float hSpeed = 6;
    float airSpeed = 9;
    float JumpSpeed = 700;
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

    public AudioSource AudioSourcePlayer;
    public AudioClip AudioClipJump;
    public AudioClip AudioClipSlow;
    public AudioClip AudioClipRewind;

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
        Hat = transform.Find("Hat Parent").gameObject;
        randomHat = Random.Range(0, 5);
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
            History.Add(new Vector4(newinput.x, newinput.y, newinput.z, GetEffectiveTime()));
        }
        input = newinput;
    }
    private void FixedUpdate()
    {
        HandleInput();
        HandleMovement();
    }
    private void Update()
    {
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

            FaceDirection( (input.x == 0) ? FaceRight : (input.x>0));
            rigidbody.velocity = new Vector2(input.x * GetMovementSpeed(), rigidbody.velocity.y);

            if (input.y > 0)
            {
                if (last_jump < Time.time)
                {
                    last_jump = Time.time + .1f;
                    rigidbody.AddForce(transform.up * JumpSpeed * rigidbody.mass);
                    AudioSourcePlayer.PlayOneShot(AudioClipJump);
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
            rigidbody.velocity += new  Vector2(input.x, input.y*.66f) * airSpeed * Time.deltaTime;
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
        bool grounded = last_grounded+.05f > Time.time;
        animator.SetBool("slowed", IsSlowed());
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
/*
    public virtual void Kill()
    {
        if (!registermovement)
        {
            return;
        }
        defeated = true;
        enabled = false;
        registermovement = false;
        History.Add(new Vector4(input.x, input.y, input.z, GetEffectiveTime()));
        History.Add(new Vector4(0, 0, 0, -1));
        Explode();
    }
    public void Explode()
    {
        GameObject poof = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Effects/Explosion"));
        poof.transform.position = transform.position;
    }*/
    public override void TimeReset()
    {
        last_jump = -1;
        last_interact = -1;
        if (!defeated && registermovement)
        {
            if (recordstate == 1)
            {
                History.Add(new Vector4(input.x, input.y, input.z, GetEffectiveTime()));
            }

            registermovement = true;
        }
        delay = 0;
        slowedTime = 0;
        EndInteraction(interactable);
        myPickup = null;
        base.TimeReset();
        if (IsOriginal())
        {
            EquiptRandomHat();
        }
    }
    public float GetEffectiveTime()
    {
        return Time.time - SpawnTime;
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
        AudioSourcePlayer.PlayOneShot(AudioClipSlow);
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
        if (t == 0)
        {
            return;
        }
        delay += t;
        slowedTime -= t;
        animator.SetTrigger("Hurt");

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
        EndInteraction(interactable);
        last_interact = Time.time + .3f;
    }
    public void DropItem(bool thrown)
    {
        if (myPickup != null)
        {
            myPickup.ChangeState(true);
            if (thrown)
            {
                myPickup.rigidbody.velocity = new Vector2(3 * (FaceRight ? 1 : -1), 5);
                myPickup.AudioSourceApple.PlayOneShot(myPickup.AudioClipAppleThrow);
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
    public void StartInteraction(TimeInteractable mObject)
    {
        interactable = mObject;
        if (IsOriginal() && mObject.Annotation != null)
            mObject.Annotation.SetActive(true);
    }
    public void EndInteraction(TimeInteractable mObject)
    {
        if (interactable == mObject)
        {
            if (mObject!=null && mObject.Annotation != null)
                mObject.Annotation.SetActive(false);
            interactable = null;
        }
    }
    public GameObject Hat;
    public void EquiptRandomHat()
    {
        SpriteRenderer HatSprite = Hat.GetComponentInChildren<SpriteRenderer>();
        randomHat += (randomHat == 5) ? -5 : 1;
        HatSprite.sprite = Resources.LoadAll<Sprite>("Textures/Player/hat")[randomHat];
    }
    public override void FaceDirection(bool right)
    {
        base.FaceDirection(right);
        Hat.transform.localScale = new Vector3(right ? 1 : -1, 1, 1);
    }
}
