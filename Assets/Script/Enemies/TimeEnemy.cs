using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : TimeCritter
{
    //--------------------
    //  TIME ENEMY
    //--------------------
    //This script controlls basic enemy behavior

    //  REGIONS
    //  Monobehavior - Declarations, and 
    //  Movement - handle movement if not stunned
    //  TimeStuff - TimeEntity related
    //  Stuns - set creature stunned, and stun time
    //  Collision - turn when touching walls, and die when crushed by large objects
    //  Death - die with a nice explosion particle, and reset/revive

    #region Monobehavior
    protected override void Awake()
    {
        base.Awake();
        GameObject deathefx = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Enemy Death Particle"));
        deathefx.SetActive(false);
        Deathplosion = deathefx.GetComponent<ParticleSystem>();
        offset = GetComponent<CircleCollider2D>().offset;
    }
    private void Update()
    {
        Walk();
    }
    #endregion
    #region Movement
    public float MoveSpeed = 1f;
    protected bool Walking = true;
    public void Walk()
    {
        Vector2 velocity = rigidbody.velocity;

        if (IsStunned())
        {
            velocity.x = 0;
        }
        else
        {
            if (rigidbody.IsSleeping())
            {
                rigidbody.WakeUp();
            }
            if (IsGrounded() && Walking)
            {
                velocity.x = ((FaceRight) ? 1 : -1) * MoveSpeed;
            }
        }
        rigidbody.velocity = velocity;
    }
    #endregion
    #region TimeStuff
    public override void TimeReset()
    {
        UnDie();
        base.TimeReset();
        Walking = true;
        stunTime = 0;
        StopAllCoroutines();
    }
    #endregion
    #region Stuns
    public bool IsStunned()
    {
        return stunTime > Time.time;
    }
    public void Pause(float pausetime)
    {
        stunTime = Time.time + pausetime;
    }
    protected float stunTime = 0;
    #endregion
    #region Collision
    Vector2 offset;
    public override void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.point.y < transform.position.y - Height * .5f + offset.y)
            {
                last_grounded = Time.time + .1f;
            }
            else if ((FaceRight && contact.point.x > transform.position.x) || (!FaceRight && contact.point.x < transform.position.x))
            {
                FaceDirection(!FaceRight);
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Interactable" )

        {
            float totalForce = 0;

            for (int iC = 0; iC< collision.contacts.Length; iC++)
            {
                totalForce += collision.contacts[iC].normalImpulse;
            }
            if (totalForce> KillImpulse)
            { 
            Die();
            }
        }
    }
    #endregion
    #region Death
    public ParticleSystem Deathplosion;
    public float KillImpulse = 15;
    public void Die()
    {
        gameObject.SetActive(false);
        if (Deathplosion!=null)
        {
            Deathplosion.gameObject.SetActive(true);
            Deathplosion.transform.position = transform.position;
            Deathplosion.Play();
        }
    }
    public void UnDie()
    {
        gameObject.SetActive(true);
    }
    #endregion
}
