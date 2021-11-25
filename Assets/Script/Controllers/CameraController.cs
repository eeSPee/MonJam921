using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool StretchTexture = true;
    GameObject followTarget;

    //--------------------
    //  CAMERA CONTROLLER
    //--------------------
    //This script makes the camera follow a gameobject

    //  VARS
    //  StretchTexture - should stretch or tile the overlay texture
    //  followTarget - gameobject being followed
    //  FUNCTIONS
    //  Awake - also in charge of the paper overlay
    //  TrackTarget - set following gameobject
    //  Update


    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        SpriteRenderer camTex = GetComponentInChildren< SpriteRenderer>();
        float aspect = Mathf.Max(1, cam.aspect);
        if (StretchTexture)
        {
            camTex.drawMode = SpriteDrawMode.Simple;
            camTex.transform.localScale = Vector3.one * cam.orthographicSize * aspect;
        }
        else
        {
            camTex.drawMode = SpriteDrawMode.Tiled;
            camTex.size = new Vector2(aspect * cam.orthographicSize, cam.orthographicSize) * 2 / camTex.transform.localScale.x;
        }
    }
    public void TrackTarget(GameObject target)
    {
        followTarget = target;
    }

    void Update()
    {
        if (followTarget!=null)
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -10);
    }
}
