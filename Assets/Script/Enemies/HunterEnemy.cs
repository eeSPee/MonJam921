using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterEnemy : TimeEnemy
{
    TimeEntity target;
    List<TimeEntity> possibleTargets = new List<TimeEntity>();
    public Vector2 sightrange = new Vector2(3,1);
    public float DefenseRadius = .2f;
    Vector2 defendPosition = Vector2.zero;
    public State currentstate = State.idle;
   public  enum State
    {
        idle = 0,
        hunting,
        retreating,
        eating
    }
    void Start()
    {
        defendPosition = transform.position;
        ScanForTargets();
    }
    public override void TimeReset()
    {
        base.TimeReset();
        ScanForTargets();
        ChangeState(State.idle);
        lastStateUpdate = 0;
        target = null;
    }
    void Update()
    {
        HandleState();
        HandleMovement();
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
        switch (currentstate)
        {
            case State.idle:
            case State.retreating:
                foreach (TimeEntity possibleTarget in possibleTargets)
                {
                    if (IsInSight(possibleTarget.gameObject))
                    {
                        target = possibleTarget;
                        currentstate = State.hunting;
                        break;
                    }
                }
                if (currentstate == State.retreating && Mathf.Abs(transform.position.x - defendPosition.x) < DefenseRadius)
                {
                    currentstate = State.idle;
                }
                break;
            case State.hunting:
                if (target==null || !IsInSight(target.gameObject))
                {
                    currentstate = State.retreating;
                    return;
                }
                break;
            case State.eating:
                Pause(1);
                rigidbody.velocity = Vector2.zero;
                break;
        }
    }
    public bool IsInSight(GameObject target)
    {
        return Mathf.Abs(transform.position.y - target.transform.position.y) <= sightrange.y && Mathf.Abs(transform.position.x - target.transform.position.x) <= sightrange.x;
    }
    void HandleMovement()
    {
        Vector2 velocity = rigidbody.velocity;

        if (IsGrounded())
        {
            if (stunTime > Time.time)
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
                            movement = 0;
                        }
                        else
                        {
                            movement *= .33f;
                            if ((FaceRight && transform.position.x > defendPosition.x + DefenseRadius) || (!FaceRight && transform.position.x < defendPosition.x - DefenseRadius))
                            {
                                FaceRight = !FaceRight;
                            }
                        }
                        break;
                    case State.retreating:
                        movement *= .5f;
                        FaceRight = transform.position.x < defendPosition.x;
                        break;
                    case State.hunting:
                        if (target == null)
                        {
                            ChangeState(State.idle);
                        }
                        FaceRight = transform.position.x < target.transform.position.x;
                        break;
                }
                velocity.x = (FaceRight ? 1 : -1) * movement;
            }
        }
        rigidbody.velocity = velocity;
    }
    public void ChangeState(State newState)
    {
        currentstate = newState;
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
    public float AttackDelay = 1;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (stunTime < Time.time)
        {
            if (collision.gameObject.tag == "Player")
            {
                PlayerController player = collision.gameObject.GetComponent<PlayerController>();
                if (player != null)
                {
                    //player.KnockBack(10 * (transform.position.x < player.transform.position.x ? 1 : -1), 10);
                    player.Delay(AttackDelay);
                    Pause(5 + (player.IsOriginal() ? 0 : AttackDelay));
                    target = null;
                    ChangeState(State.retreating);
                }
            }
            else if (collision.gameObject.tag == "Bait")
            {
                TimeDroppable bait = collision.gameObject.GetComponent<TimeDroppable>();
                if (bait != null && bait.state  && bait.rigidbody.velocity.sqrMagnitude<.4f)
                {
                    ChangeState(State.eating);
                    bait.ChangeState(false);
                }
            }
        }
    }
}
