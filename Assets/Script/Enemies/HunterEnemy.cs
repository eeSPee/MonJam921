using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterEnemy : TimeEnemy
{
    //--------------------
    //  HUNTER ENEMY
    //--------------------
    //This script controlls the behavior of the spider enemies

    //  REGIONS
    //  AiState - handles various ai states; idle, hunting, retreating, eating
    //  Monobehavior - Monobehavior Calls
    //  Targeting - Scan for player target, is player in range, and forget target
    //  Defense - In the retreating/idle state, the spider will patrol an area
    //  Time Stuff - Time Imprint and Reset
    //  Attack - pause the player when it touches our collider, or enter the sleeping state when reaching an apple
    //  Audio - play sound when player touched

    #region AiState
    public State currentstate = State.idle;
   public  enum State
    {
        idle = 0,
        standing,
        hunting,
        retreating,
        eating
    }
    float lastStateUpdate = 0;
    void HandleState()
    {
        if (lastStateUpdate > Time.time)
            return;
        lastStateUpdate = Time.time + .33f;
        if (rigidbody.IsSleeping())
        {
            rigidbody.WakeUp();
        }
        if (IsStunned())
        {
            return;
        }
        switch (currentstate)
        {
            case State.idle:
            case State.standing:
            case State.retreating:
                foreach (TimeEntity possibleTarget in possibleTargets)
                {
                    if (IsInSight(possibleTarget.gameObject))
                    {
                        target = possibleTarget;
                        ChangeState(State.hunting);
                        if (target.gameObject.tag == "Bait")
                        {
                            AudioSourceSpider.PlayOneShot(AudioClipGrowlBait);
                        }
                        else if (target.gameObject.tag == "Player")
                        {
                            AudioSourceSpider.PlayOneShot(AudioClipGrowlPrey);
                        }
                        break;
                    }
                }
                if (currentstate == State.retreating && Mathf.Abs(transform.position.x - defendPosition.x) < Mathf.Max(DefenseRadius, 1))
                {
                    ChangeState(State.idle);
                }
                break;
            case State.hunting:
                if (target == null || !IsInSight(target.gameObject))
                {
                    ChangeState(State.retreating);
                    return;
                }
                break;
        }
    }
    public void ChangeState(State newState)
    {
        currentstate = newState;
        switch (currentstate)
        {
            case State.idle:
                animator.SetFloat("walk_speed", .33f);
                animator.SetBool("chewing", false);
                break;
            case State.standing:
                animator.SetFloat("walk_speed", 0);
                animator.SetBool("chewing", false);
                break;
            case State.retreating:
                animator.SetFloat("walk_speed", .5f);
                animator.SetBool("chewing", false);
                break;
            case State.hunting:
                animator.SetFloat("walk_speed", 1);
                animator.SetBool("chewing", false);
                break;
            case State.eating:
                animator.SetBool("chewing", true);
                Pause(100);
                break;
        }
    }
    void HandleMovement()
    {
        Vector2 velocity = rigidbody.velocity;

        if (IsGrounded())
        {
            if (IsStunned())
            {
                velocity.x = 0;
            }
            else if (Walking)
            {
                float movement = MoveSpeed;
                switch (currentstate)
                {
                    case State.idle:
                        if (DefenseRadius == 0)
                        {
                            ChangeState(State.standing);
                        }
                        else
                        {
                            movement *= .33f;
                            if ((FaceRight && transform.position.x > defendPosition.x + DefenseRadius) || (!FaceRight && transform.position.x < defendPosition.x - DefenseRadius))
                            {
                                FaceDirection(!FaceRight);
                            }
                        }
                        break;
                    case State.standing:
                        if (!IsStunned() && DefenseRadius > 0)
                        {
                            ChangeState(State.retreating);
                        }
                        break;
                    case State.retreating:
                        movement *= .5f;
                        FaceDirection(transform.position.x < defendPosition.x);
                        break;
                    case State.hunting:
                        if (target == null)
                        {
                            ChangeState(State.idle);
                        }
                        FaceDirection(transform.position.x < target.transform.position.x);
                        break;
                }
                velocity.x = (FaceRight ? 1 : -1) * movement;
            }
        }
        rigidbody.velocity = velocity;
    }
    #endregion
    #region MonoBehavior
    void Start()
    {
        defendPosition = transform.position;
        ScanForTargets();
    }
    void Update()
    {
        HandleState();
        HandleMovement();
        UpdateListener();
    }
    #endregion
    #region Targeting
    public Vector2 sightrange = new Vector2(3, 1);
    TimeEntity target;
    List<TimeEntity> possibleTargets = new List<TimeEntity>();
    void ForgetTarget()
    {
        target = null;
    }
    public bool IsInSight(GameObject target)
    {
        return target.activeSelf && Mathf.Abs(transform.position.y - target.transform.position.y) <= sightrange.y && Mathf.Abs(transform.position.x - target.transform.position.x) <= sightrange.x;
    }
    public void ScanForTargets()
    {
        foreach (PlayerController prey in GameObject.FindObjectsOfType<PlayerController>())
        {
            if (!possibleTargets.Contains(prey))
            {
                possibleTargets.Add(prey);
            }
        }
        foreach (TimeDroppable bait in GameObject.FindObjectsOfType<TimeDroppable>())
        {
            if (bait.gameObject.tag == "Bait" && !possibleTargets.Contains(bait))
            {
                possibleTargets.Add(bait);
            }
        }
    }
    #endregion
    #region Defense
    public float DefenseRadius = .2f;
    Vector2 defendPosition = Vector2.zero;
    #endregion
    #region Time Stuff
    public override void TimeReset()
    {
        base.TimeReset();
        ScanForTargets();
        ChangeState(State.idle);
        lastStateUpdate = 0;
        ForgetTarget();
    }

    #endregion
    #region Attack
    public float AttackDelay = 1;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!IsStunned())
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    rigidbody.velocity = Vector2.zero;
                    //player.KnockBack(10 * (transform.position.x < player.transform.position.x ? 1 : -1), 10);
                    player.Delay(AttackDelay);
                    //Pause(5 + (player.IsOriginal() ? 0 : AttackDelay));
                    Pause(5 + AttackDelay);
                    ForgetTarget();
                    animator.SetTrigger("Bite");
                    AudioSourceSpider.PlayOneShot(AudioClipBite);
                    ChangeState(State.standing);
                }
            }
            else if (collision.gameObject.tag == "Bait")
            {
                TimeDroppable bait = collision.gameObject.GetComponent<TimeDroppable>();
                if (bait != null && bait.state)
                {
                    ChangeState(State.eating);
                    bait.ChangeState(false);
                    bait.gameObject.SetActive(false);
                }
            }
        }
    }
    #endregion
    #region Audio

    public AudioSource AudioSourceSpider;
    public AudioClip AudioClipGrowlBait;
    public AudioClip AudioClipGrowlPrey;
    public AudioClip AudioClipBite;
    public void UpdateListener()
    {
		float ListenerDistance = ((Vector2)transform.position - (Vector2)PlayerController.player.transform.position).magnitude;
		AudioSourceSpider.volume = Mathf.Clamp(1 - ((ListenerDistance-GameController.MinListenerDistance) / (GameController.MaxListenerDistance - GameController.MinListenerDistance)),0,1);
    }
    #endregion
}
