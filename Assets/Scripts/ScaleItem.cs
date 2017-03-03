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
        var ratio = textureWidth / textureHeight;
        var scale = PixelPerfectCamera.scale;

        var newWidth = Mathf.Ceil(((textureWidth) * PixelPerfectCamera.scale));
        var newHeight = Mathf.Ceil(((textureHeight) * PixelPerfectCamera.scale));

        //Adjust box collider
        UpdateCollider();

        //This sets the scale of the background transform to coorespond to the screen
        transform.localScale = new Vector3((newWidth/textureWidth), (newHeight/textureHeight), 2);

     //   GetComponent<Renderer>().material.mainTextureScale = new Vector3((newWidth/textureWidth), (newHeight/textureHeight), 2);
    }

    void UpdateCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().offset = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center;
    }
}
