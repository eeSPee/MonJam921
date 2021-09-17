using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeEnemy : TimeCritter
{
    public ParticleSystem Deathplosion;
    public float MoveSpeed = 1f;
    protected bool Walking = true;
    protected float stunTime = 0;
    protected override void Awake()
    {
        base.Awake();
        GameObject deathefx = Instantiate(Resources.Load<GameObject>("Prefabs/Enemies/Enemy Death Particle"));
        deathefx.SetActive(false);
        Deathplosion = deathefx.GetComponent<ParticleSystem>();
    }
    public override void TimeReset()
    {
        UnDie();
        base.TimeReset();
        Walking = true;
        stunTime = 0;
        StopAllCoroutines();
    }
    public bool IsStunned()
    {
        return stunTime > Time.time;
    }
    private void Update()
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
    public void Pause(float pausetime)
    {
        stunTime = Time.time+pausetime;
    }
    public override void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.point.y < transform.position.y - Height * .5f)
            {
                last_grounded = Time.time + .1f;
                break;
            }
            else if ((FaceRight && contact.point.x > transform.position.x) || (!FaceRight && contact.point.x < transform.position.x))
            {
                FaceDirection(!FaceRight);
                break;
            }
        }
    }
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Interactable" && collision.contacts[0].normalImpulse > 7f)
        {
            Die();
        }
    }
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
}
