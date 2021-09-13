using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDroppable : TimeInput
{
    Collider2D[] colliders;
    protected override void Awake()
    {
        base.Awake();
        colliders = new Collider2D[]
        {
            GetComponent<Collider2D>(),
            GetComponentInChildren<Collider2D>()
        };
    }
    public override void PlayerInteract(PlayerController player)
    {
        player.PickUpItem(this);
    }
    public override void ChangeState(bool value)
    {
        rigidbody.simulated = value;
        foreach (Collider2D component in colliders)
        {
            component.enabled = value;
        }
    }
}
