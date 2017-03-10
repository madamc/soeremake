using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JibberJabber : MonoBehaviour {
    public string textToShow;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void setTextandShow()
    {
        this.gameObject.GetComponent<Text>().text = textToShow;
        this.gameObject.SetActive(true);
    }
}
