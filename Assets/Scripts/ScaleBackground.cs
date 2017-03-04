using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleBackground : MonoBehaviour {
    public int textureHeight = 240;
    public int textureWidth = 427;
    // Use this for initialization
    void Start () {
        var newWidth = Mathf.Ceil(Screen.width / (textureWidth * PixelPerfectCamera.scale));
        var newHeight = Mathf.Ceil(Screen.height / (textureHeight * PixelPerfectCamera.scale));

        //This sets the scale of the background transform to coorespond to the screen
        transform.localScale = new Vector3(newWidth*textureWidth,newHeight*textureHeight,1);

    //    GetComponent<Renderer>().material.mainTextureScale = new Vector3(newWidth, newHeight, 1);
    }

}
