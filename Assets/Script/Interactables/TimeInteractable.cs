using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TimeInteractable : TimeEntity
{
    //--------------------
    //  TIME INTERACTABLE
    //--------------------
    //This script controlls pickups that reset when the game resets

    //  VARS
    //  state - was this item touched by player?
    //  Start_State - state when reset
    //  Annotation - the little "Pick me Up" text that appears on top when player walks over
    //  FUNCTIONS
    //  Awake - declarations
    //  TimeImprint - See TimeEntity
    //  TimeReset - See TimeEntity
    //  ChangeState - Set enabled/disabled
    //  PlayerInteract - Called when player uses the 'interact' key

    public bool state = false;
    bool Start_State = false;
    public GameObject Annotation;

    protected override void Awake()
    {
        base.Awake();
        Annotation = transform.Find("Annotation")?.gameObject;
        if (Annotation != null)
            Annotation.SetActive(false);
    }
    public override void TimeImprint()
    {
        base.TimeImprint();
        Start_State = state;
    }
    public override void TimeReset()
    {
        gameObject.SetActive(true);
        base.TimeReset();
        ChangeState(Start_State);
    }
    public virtual void ChangeState(bool value)
    {
        state = value;
    }
    public virtual void PlayerInteract(PlayerController player)
    {
        ChangeState(!state);
    }

}
