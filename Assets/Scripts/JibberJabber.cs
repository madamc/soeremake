using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JibberJabber : MonoBehaviour {
    public string textToShow;
    public bool isHid;
	// Use this for initialization
	void Start () {
        gameObject.GetComponentInParent<Transform>().gameObject.SetActive(false);
        isHid = true;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

   public void setTextandShow(string text)
    {
       
        textToShow = text;
        this.gameObject.GetComponentInChildren<Text>().text = textToShow;
       
        gameObject.GetComponentInParent<Transform>().gameObject.SetActive(true);
        this.gameObject.SetActive(true);
        Debug.Log(this.gameObject.GetComponentInParent<Transform>().position.z);
        this.gameObject.GetComponentInParent<RectTransform>().position = new Vector3(0, 70, -1);
        Debug.Log(this.gameObject.GetComponentInParent<Transform>().position.z);
        isHid = false;
    }

    public void hideme()
    {
        textToShow = "";
        this.gameObject.GetComponentInChildren<Text>().text = textToShow;
        gameObject.GetComponentInParent<Transform>().gameObject.SetActive(false);
        isHid = true;
    }
}
