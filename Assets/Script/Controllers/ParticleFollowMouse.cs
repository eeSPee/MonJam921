using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleFollowMouse : MonoBehaviour
{

    void FixedUpdate()
    {
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
}
