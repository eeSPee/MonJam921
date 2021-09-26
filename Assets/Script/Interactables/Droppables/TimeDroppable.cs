using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeDroppable : TimeInput
{
  public AudioSource AudioSourceApple;
  public AudioClip AudioClipApplePickup;
  public AudioClip AudioClipAppleThrow;

  private void Update()
  {
    UpdateListener();
  }
  
    public void UpdateListener()
    {
		float ListenerDistance = ((Vector2)transform.position - (Vector2)PlayerController.player.transform.position).magnitude;
		AudioSourceApple.volume = Mathf.Clamp(1 - ((ListenerDistance-GameController.MinListenerDistance) / (GameController.MaxListenerDistance - GameController.MinListenerDistance)),0,1);
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
