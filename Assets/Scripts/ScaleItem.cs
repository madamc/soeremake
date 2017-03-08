using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleItem : MonoBehaviour {

    public float textureHeight;
    public float textureWidth;
    // Use this for initialization
    void Start()
    {


        //float depth = gameObject.transform.lossyScale.z;
        //float width = gameObject.transform.lossyScale.x;
        //float height = gameObject.transform.lossyScale.y;

        //Vector3 lowerLeftPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x - width / 2, gameObject.transform.position.y - height / 2, gameObject.transform.position.z - depth / 2));
        //Vector3 upperRightPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + width / 2, gameObject.transform.position.y + height / 2, gameObject.transform.position.z - depth / 2));
        //Vector3 upperLeftPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x - width / 2, gameObject.transform.position.y + height / 2, gameObject.transform.position.z - depth / 2));
        //Vector3 lowerRightPoint = Camera.main.WorldToScreenPoint(new Vector3(gameObject.transform.position.x + width / 2, gameObject.transform.position.y - height / 2, gameObject.transform.position.z - depth / 2));

        //float xPixelDistance = Mathf.Abs(lowerLeftPoint.x - upperRightPoint.x);
        //float yPixelDistance = Mathf.Abs(lowerLeftPoint.y - upperRightPoint.y);

        //Debug.Log("X pixel dist" +xPixelDistance);
        //Debug.Log("Y Pixel dist" + yPixelDistance);
       

        //textureHeight = GetComponent<Renderer>().bounds.size.y;
        //textureWidth = GetComponent<Renderer>().bounds.size.x;
        //var ratio = textureHeight / textureWidth;
        var scale = PixelPerfectCamera.scale;
        Debug.Log("scale: " + scale);
        //var newWidth = Mathf.Ceil(((textureWidth) * scale));
        //var newHeight = Mathf.Ceil(((textureHeight) * scale));


        //This sets the scale of the background transform to coorespond to the screen
      //  transform.localScale = new Vector3(scale/xPixelDistance, scale/yPixelDistance, 5);

        //Adjust box collider
        UpdateCollider();

      

        Debug.Log("resized texture: " + GetComponent<Renderer>().bounds.size.y);
     //   GetComponent<Renderer>().material.mainTextureScale = new Vector3((newWidth/textureWidth), (newHeight/textureHeight), 2);
    }

    void UpdateCollider()
    {
        gameObject.GetComponent<BoxCollider2D>().size = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().offset = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.center;
    }
}
