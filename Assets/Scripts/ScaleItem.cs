using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleItem : MonoBehaviour {

    public float textureHeight;
    public float textureWidth;
    // Use this for initialization
    void Start()
    {
        textureHeight = GetComponent<Renderer>().bounds.size.y;
        textureWidth = GetComponent<Renderer>().bounds.size.x;

        var newWidth = Mathf.Ceil(Screen.width / (textureWidth * PixelPerfectCamera.scale));
        var newHeight = Mathf.Ceil(Screen.height / (textureHeight * PixelPerfectCamera.scale));

        //This sets the scale of the background transform to coorespond to the screen
        transform.localScale = new Vector3(newWidth * textureWidth, newHeight * textureHeight, 2);

        GetComponent<Renderer>().material.mainTextureScale = new Vector3(newWidth, newHeight, 2);
    }
}
