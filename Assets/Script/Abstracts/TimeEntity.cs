using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TimeEntity : MonoBehaviour
{
    public Vector3 Start_Position;
    public Quaternion Start_Rotation;

    public Rigidbody2D rigidbody;
    public Animator animator;
    public Vector2 StartVelocity;
    public float StartAVelocity;
    protected virtual void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        TimeImprint();
    }
    public virtual void TimeImprint()
    {
        Start_Position = transform.position;
        Start_Rotation = transform.rotation;
        StartVelocity = rigidbody.velocity;
        StartAVelocity = rigidbody.angularVelocity;
    }
    public virtual void TimeReset()
    {
        transform.position = Start_Position;
        transform.rotation = Start_Rotation;

        rigidbody.velocity = StartVelocity;
        rigidbody.angularVelocity = StartAVelocity;
        rigidbody.WakeUp();
    }
}
