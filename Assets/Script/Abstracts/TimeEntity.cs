using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TimeEntity : MonoBehaviour
{
    //--------------------
    //  TIME ENTITY
    //--------------------
    //This script controlls entities and time resets

    //  VARS
    //  Start_Position, Start_Rotation, StartVelocity, StartAVelocity - initial velocity or position of this entity
    //  rigidbody, animator - attached components
    //  FUNCTIONS
    //  Awake - declares components and imprints on time
    //  TimeImprint - stores the start values to the current values of this object
    //  TimeReset - resets the position/velocity of this objects to the values stored

    public RigidbodyType2D RigidBody_Type;
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
        RigidBody_Type = rigidbody.bodyType;
    }
    public virtual void TimeReset()
    {
        rigidbody.bodyType = RigidbodyType2D.Kinematic;
        transform.position = Start_Position;
        transform.rotation = Start_Rotation;

        rigidbody.velocity = StartVelocity;
        rigidbody.angularVelocity = StartAVelocity;
        rigidbody.bodyType = RigidBody_Type;
        rigidbody.WakeUp();
    }
}
