using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDroppable : TimeInput
{
  public AudioSource AudioSourceApple;
  public AudioClip AudioClipApplePickup;
  public AudioClip AudioClipAppleThrow;

  public float MinListenerDistance=1;
  public float MaxListenerDistance=25;

  private void Update()
  {
    UpdateListener();
  }

  public void UpdateListener()
  {
    float ListenerDistance = Vector3.Distance(transform.position, PlayerController.player.transform.position);

    if (ListenerDistance <= MinListenerDistance)
    {
      AudioSourceApple.volume = 1;
    }
    else if (ListenerDistance > MaxListenerDistance)
    {
      AudioSourceApple.volume = 0;
    }
    else
    {
      AudioSourceApple.volume = 1 - ((ListenerDistance-MinListenerDistance) / (MaxListenerDistance - MinListenerDistance));
    }
  }

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
        AudioSourceApple.PlayOneShot(AudioClipApplePickup);
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
