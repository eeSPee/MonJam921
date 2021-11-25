using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowMouse : MonoBehaviour
{
    //--------------------
    //  PARTICLE FOLLOW MOUSE
    //--------------------
    //This script controlls mouse particles in the main menu (unused?)

    //  FUNCTIONS
    //  FixedUpdate - changes to mouse position

    void FixedUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
