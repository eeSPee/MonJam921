using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : PlayerController
{
    protected override void Awake()
    {
        Display = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        recordstate = 0;
        registermovement = false;
    }
    protected override void HandleInput()
    {
        if (recordstate >= 0 && recordstate < History.Count)
        {
            if (History[recordstate].w < 0)
            {
                Kill();
            }
            if (Time.time >= History[recordstate].w + SpawnTime)
            {
                input = new Vector3(History[recordstate].x, History[recordstate].y, History[recordstate].z);
                recordstate++;
            }
        }
    }
    public void MimicPlayer(PlayerController other)
    {
        name = "Clone of " + other.name + " at "+ Time.time;

        History = new List<Vector4>();
        History.AddRange(other.History);

        Start_Position = other.Start_Position;
        Start_Rotation = other.Start_Rotation;
        StartVelocity = other.StartVelocity;
        StartAVelocity = other.StartAVelocity;
    }
    public override void Kill()
    {
        enabled = false;
        defeated = true;
        Explode();
    }
    public override void TimeReset()
    {
        registermovement = false;
        base.TimeReset();

        SpawnTime = Time.time;
        recordstate = 0;
        animator.SetBool("stunned", false);
    }
    Vector2 pastvelocity = Vector2.zero;
    public override void Delay(float t)
    {
        if (PauseCoroutine != null)
        {
            StopCoroutine(PauseCoroutine);
            rigidbody.velocity = pastvelocity;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("stunned", false);
        }

        PauseCoroutine = StartCoroutine(Pause(t));
    }
    Coroutine PauseCoroutine;
    public IEnumerator Pause(float duration)
    {
        Debug.Log("Pause " + name + " with velocity " + rigidbody.velocity);
        animator.SetBool("stunned", true);
        pastvelocity = rigidbody.velocity;
        rigidbody.bodyType = RigidbodyType2D.Static;
        yield return new WaitForSeconds(duration);
        rigidbody.bodyType = RigidbodyType2D.Dynamic;
        rigidbody.velocity = pastvelocity;
        animator.SetBool("stunned", false);
    }
    public override bool IsOriginal()
    {
        return false;
    }
}