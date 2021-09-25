using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public AudioSource AudioSourceMushroom;
    public AudioClip[] AudioClipMushroom;

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
        AudioSourceMushroom.volume = 1;
      }
      else if (ListenerDistance > MaxListenerDistance)
      {
        AudioSourceMushroom.volume = 0;
      }
      else
      {
        AudioSourceMushroom.volume = 1 - ((ListenerDistance-MinListenerDistance) / (MaxListenerDistance - MinListenerDistance));
      }
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
            //AudioSourceMushroom.clip = AudioClipMushroom[Random.Range(0, AudioClipMushroom.Length)];
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
