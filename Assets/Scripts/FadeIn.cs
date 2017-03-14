using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour {

    public bool isFadingIn=false;
    public float alphamax = 255f;
    private float currentalpha = 0.0f;
	// Use this for initialization
	void Start () {
        Color currentcolor = gameObject.GetComponent<SpriteRenderer>().color;
        currentcolor = new Color(currentcolor.r, currentcolor.g, currentcolor.b, 0.0f);

    }
	
	// Update is called once per frame
	void Update () {
        if (isFadingIn ==true &&currentalpha!= alphamax) {
            Color currentcolor = gameObject.GetComponent<SpriteRenderer>().color;
            currentalpha = currentcolor.a;
            float nextalpha = currentalpha + Time.deltaTime;

            currentcolor = new Color(currentcolor.r, currentcolor.g, currentcolor.b, nextalpha);
        }
	}
}
