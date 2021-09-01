using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneController : PlayerController
{
    protected override void Awake()
    {
        Display = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        recordstate = 0;
    }
    protected override void Update()
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
        transform.position = Start_Position;
        transform.rotation = Start_Rotation;

        rigidbody.velocity = StartVelocity;
        rigidbody.angularVelocity = StartAVelocity;
        SpawnTime = Time.time;
        recordstate = 0;
        interactable = null;
    }
}