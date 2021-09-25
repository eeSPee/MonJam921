using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public bool StretchTexture = true;
    GameObject followTarget;

    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        SpriteRenderer camTex = GetComponentInChildren< SpriteRenderer>();
        if (StretchTexture)
        {
            camTex.drawMode = SpriteDrawMode.Simple;
            camTex.transform.localScale = Vector3.one * cam.orthographicSize * cam.aspect;
        }
        else
        {
            camTex.drawMode = SpriteDrawMode.Tiled;
            camTex.size = new Vector2(cam.aspect * cam.orthographicSize, cam.orthographicSize) * 2 / camTex.transform.localScale.x;
        }
    }
    public void TrackTarget(GameObject target)
    {
        followTarget = target;
    }

    void FixedUpdate()
    {
        if (followTarget!=null)
            transform.position = new Vector3(followTarget.transform.position.x, followTarget.transform.position.y, -10);
    }
}
