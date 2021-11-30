using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController : TimeCritter
{
    //--------------------
    //  CLONE CONTROLLER
    //--------------------
    //This script controlls the player

    //  REGIONS
    //  Monobehavior - Declarations on awake, and frame by frame
    //  Input - handle player imput, and store it in History
    //  Cloning - clone from player, and override isOriginal
    //  Movement - walking, jumping
    //  Animatior - handle player animations (jump, fall, walk, etc)
    //  Time Stuff - Time Entity Stuff
    //  History - Record movement and handle delays
    //  Crowd Control - Handles slows and player pauses when the player is hit by enemies/deathzone
    //  Clones - clone related, and checks for original
    //  Pickups - handles pickups and picking up/carrying items like apples
    //  Interactions - Interact with switches and other objects
    //  Hats - visual assign a hat to the player and clones
    //  Audio - audio related stuff

    #region MonoBehavior
    public static PlayerController player;
    protected SpriteRenderer Display;
    protected Collider2D Collision;
    protected override void Awake()
    {
      Display = GetComponent<SpriteRenderer>();
        Collision = GetComponent<Collider2D>();
        base.Awake();
        RewriteHistory();
        registermovement = true;
        SpawnTime = Time.time;
        last_grounded = Time.time + .1f;
        Hat = transform.Find("Hat Parent").gameObject;
        randomHat = Random.Range(0, 5);
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
        UpdateListener();
    }
    #endregion
    #region Input
    public Vector3 input = Vector3.zero;
    public int recordstate = 0;
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
    #endregion
    #region Movement
    Vector2 MoveSpeed = new Vector2(6, 6);
    float JumpSpeed = 12;
    public float GetMovementSpeed()
    {
        return MoveSpeed.x * (IsSlowed() ? .33f : 1f);
    }
    public float last_jump = -1;
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
                    last_jump = Time.time + .3f;
                    rigidbody.velocity = new Vector2(rigidbody.velocity .x,  JumpSpeed);
                    AudioSourcePlayer.PlayOneShot(AudioClipJump);
                }
            }
            if (input.z != 0 && last_interact < Time.time)
            {
                if (myPickup != null)
                {
                    DropItem(input.y >= 0);
                }
                else if (interactable != null)
                {
                    interactable.PlayerInteract(this);
                    animator.SetTrigger("Action");
                }
            }
        }
        else
        {
            rigidbody.velocity = new Vector2(input.x * GetMovementSpeed(), rigidbody.velocity .y + input.y * MoveSpeed.y * Time.deltaTime ) ;
            //rigidbody.velocity += new  Vector2(input.x * airSpeed.x, input.y* airSpeed.y) * Time.deltaTime;
            if (input.z != 0)
            {
                if (last_interact < Time.time)
                {
                    last_interact = Time.time + .3f;
                    if (myPickup != null)
                    {
                        DropItem(input.y >= 0);
                    }
                }
            }
        }
    }/*
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
    #endregion
    #region Animator
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
    #endregion
    
    #region Time Stuff
    public bool registermovement = false;
    public bool defeated = false;
    public override void TimeReset()
    {
        last_jump = -1;
        last_interact = -1;
        last_grounded = Time.time + .1f;
        if (!defeated && registermovement)
        {
                History.Add(new Vector4(input.x, input.y, input.z, Time.time - SpawnTime));


            registermovement = true;
        }
        delay = 0;
        slowedTime = 0;
        EndInteraction(interactable);
        myPickup = null;
        base.TimeReset();
        if (IsOriginal())
        {
            TimeUnfreeze();
            EquiptRandomHat();
        }
    }
    #endregion
    #region History
    protected float delay = 0;
    public List<Vector4> History = new List<Vector4>();
    public float GetEffectiveTime()
    {
        return Time.time - SpawnTime - delay;
    }

    public void RewriteHistory()
    {
        SpawnTime = Time.time;
        History.Clear();
        History.Add(Vector4.zero);
    }
    public float GetDelay()
    {
        return delay;
    }
    public void Delay(float t)
    {

        TimeUnfreeze();

        if (t == 0)
        {
            return;
        }
        delay += t;
        //slowedTime -= t;
        PauseCoroutine = StartCoroutine(Pause(t));
    }
    #endregion
    #region Crowd Control
    float slowedTime = 0;
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
    /*public virtual void Delay(float t)
    {
        if (t == 0)
        {
            return;
        }
        delay += t;
        slowedTime -= t;
        animator.SetTrigger("Hurt");

        Debug.Log("Delay " + name + " by " + t + " seconds");
    }*/
    Vector2 pastvelocity = Vector2.zero;
    Coroutine PauseCoroutine;
    public IEnumerator Pause(float duration)
    {
        Debug.Log("Pause " + name + " with velocity " + rigidbody.velocity);
        animator.SetBool("stunned", true);
        pastvelocity = rigidbody.velocity;
        rigidbody.bodyType = RigidbodyType2D.Static;
        animator.SetTrigger("Ouch");
        AudioSourcePlayer.PlayOneShot(AudioClipHurt);
        if (IsOriginal())
        {
            TimeFreeze();
        }
        yield return new WaitForSeconds(duration);
        if (IsOriginal())
        {
            TimeUnfreeze();
        }
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.velocity = pastvelocity;
        animator.SetBool("stunned", false);
    }
    public void TimeFreeze()
    {
        animator.SetFloat("AnimSpeed", .2f);
        if (IsOriginal())
        {
            registermovement = false;
            Time.timeScale = GameController.TimeScale * 10;
        }
    }
    public void TimeUnfreeze()
    {
        if (PauseCoroutine != null)
        {
            StopCoroutine(PauseCoroutine);
            rigidbody.velocity = pastvelocity;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("stunned", false);
            animator.SetFloat("AnimSpeed", 1);
        }
        if (IsOriginal())
        {
            registermovement = true;
            Time.timeScale = GameController.TimeScale;
        }
    }
    #endregion
    #region Clones
    public int playerID = 0;
    public float SpawnTime = 0;
    public virtual bool IsOriginal()
    {
        return true;
    }
    #endregion
    #region PickUps
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
                animator.SetTrigger("Action");
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
    #endregion
    #region Interactions
    public float last_interact = -1;
    public TimeInteractable interactable;
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
    #endregion
    #region Hats
    public static int randomHat;
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
    #endregion
    #region Audio

    public AudioSource AudioSourcePlayer;
    public AudioClip AudioClipJump;
    public AudioClip AudioClipSlow;
    public AudioClip AudioClipRewind;
    public AudioClip AudioClipHurt;
    public void UpdateListener()
    {
		if (!IsOriginal())
		{
			float ListenerDistance = ((Vector2)transform.position - (Vector2)PlayerController.player.transform.position).magnitude;
			AudioSourcePlayer.volume = Mathf.Clamp(1 - ((ListenerDistance-GameController.MinListenerDistance) / (GameController.MaxListenerDistance - GameController.MinListenerDistance)),0,1);
		}
    }
    #endregion
}
