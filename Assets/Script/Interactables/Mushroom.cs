using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    //--------------------
    //  MUSHROOM
    //--------------------
    //This script controlls mushrooms that talk to the player

    //  VARS
    //  AudioSourceMushroom, animator - components on this object
    //  AudioClipMushroom - sound to play when it talks
    //  FUNCTIONS
    //  UpdateListener - On update, updates volume based to distance to player
    //  Awake - Declarations
    //  TimeReset - See TimeEntity
    //  OnTriggerEnter2D, OnTriggerExit2D - Show/hide the speaking bubble when the player touches my trigger


    public AudioSource AudioSourceMushroom;
    public AudioClip[] AudioClipMushroom;

    private void Update()
    {
      UpdateListener();
    }

    public void UpdateListener()
    {
		float ListenerDistance = ((Vector2)transform.position - (Vector2)PlayerController.player.transform.position).magnitude;
		AudioSourceMushroom.volume = Mathf.Clamp(1 - ((ListenerDistance-GameController.MinListenerDistance) / (GameController.MaxListenerDistance - GameController.MinListenerDistance)),0,1);
    }

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.IsOriginal())
            {
            AudioSourceMushroom.PlayOneShot(AudioClipMushroom[Random.Range(0, AudioClipMushroom.Length)]);
            animator.SetBool("Speaking", true);
            animator.SetFloat("AnimSpeed", 3);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player.IsOriginal())
            {
                animator.SetBool("Speaking", false);
            animator.SetFloat("AnimSpeed", 1);
        }
    }
    }
}
