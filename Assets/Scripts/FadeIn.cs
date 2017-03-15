using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    public bool isFadingIn=false;
    public float alphamax = 1f;
    private float currentalpha = 0.0f;
    public float fadespeed = 1f;
	// Use this for initialization
	void Start () {
        Color currentcolor = gameObject.GetComponent<SpriteRenderer>().color;
        currentcolor = new Color(currentcolor.r, currentcolor.g, currentcolor.b, 0.0f);
        gameObject.GetComponent<SpriteRenderer>().color = currentcolor;

    }
	
	// Update is called once per frame
	void Update () {
        if (isFadingIn ==true &&currentalpha <= alphamax) {
            Color currentcolor = gameObject.GetComponent<SpriteRenderer>().color;
            currentalpha = currentcolor.a;
            float nextalpha = currentalpha + Time.deltaTime*fadespeed;
            Debug.Log(currentalpha);
           

            currentcolor = new Color(currentcolor.r, currentcolor.g, currentcolor.b, nextalpha);
            gameObject.GetComponent<SpriteRenderer>().color = currentcolor;
        }
	}
}
