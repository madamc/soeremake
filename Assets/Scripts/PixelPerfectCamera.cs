using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelPerfectCamera : MonoBehaviour {

    public static float pixelsToUnits = 1.0f;
    public static float scale = 1;
    public Vector2 nativeResolution = new Vector2(427, 240);
   

    private void Awake()
    {
        Camera sceneCamera = GetComponent<Camera>();
        if (sceneCamera.orthographic) {
            scale = Screen.height / nativeResolution.y;
            pixelsToUnits *= scale;
            sceneCamera.orthographicSize = (Screen.height / 2.0f) / pixelsToUnits;  
        }
}
}
