using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : PlayerController
{
    //--------------------
    //  CLONE CONTROLLER
    //--------------------
    //This script controlls clones that mimic the player's movement

    //  REGIONS
    //  Monobehavior - Declarations
    //  Input - override player input
    //  Cloning - clone from player, and override isOriginal
    //  Time Stuff - reset the clone without causing damage

    #region MonoBehavior
    protected override void Awake()
    {
        Display = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        recordstate = 0;
        registermovement = false;
        Hat = transform.Find("Hat Parent").gameObject;
    }
    #endregion
    #region Input
    protected override void HandleInput()
    {
        if (recordstate >= 0 && recordstate < History.Count)
        {
            /*if (History[recordstate].w < 0)
            {
                Kill();
            }*/
            if (Time.time >= History[recordstate].w + SpawnTime + delay)
            {
                input = new Vector3(History[recordstate].x, History[recordstate].y, History[recordstate].z);
                recordstate++;
            }
        }
    }
    #endregion
    #region Cloning
    public void MimicPlayer(PlayerController other)
    {
        name = "Clone of " + other.name + " at "+ Time.time;

        History = new List<Vector4>();
        History.AddRange(other.History);

        Start_Position = other.Start_Position;
        Start_Rotation = other.Start_Rotation;
        StartVelocity = other.StartVelocity;
        StartAVelocity = other.StartAVelocity;

        Hat.GetComponentInChildren<SpriteRenderer>().sprite = other.Hat.GetComponentInChildren<SpriteRenderer>().sprite;
    }
    public override bool IsOriginal()
    {
        return false;
    }
    #endregion
    #region Time Stuff
    public override void TimeReset()
    {
        registermovement = false;
        base.TimeReset();

        SpawnTime = Time.time;
        recordstate = 0;
        Delay(0);
        delay = 0;
        animator.SetBool("stunned", false);
    }
    /*Vector2 pastvelocity = Vector2.zero;
    public override void Delay(float t)
    {
        if (PauseCoroutine != null)
        {
            StopCoroutine(PauseCoroutine);
            rigidbody.velocity = pastvelocity;
            rigidbody.bodyType = RigidbodyType2D.Dynamic;
            animator.SetBool("stunned", false);
        }
        if (t == 0)
        {
            return;
        }
        delay += t;
        slowedTime -= t;
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
    }*/
    #endregion
}