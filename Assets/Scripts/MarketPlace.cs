using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketPlace : MonoBehaviour {


    // Use this for initialization
    void Awake()
    {
        print("This getting caleed!");
        var couch = GameObject.Find("couch").GetComponent<SceneItem>();
        var chair = GameObject.Find("chair").GetComponent<SceneItem>();
        var crate = GameObject.Find("crate").GetComponent<SceneItem>();

        couch.GiveKeyValue("IMACOUCH");
        chair.GiveKeyValue("IMACHAIR");
        crate.GiveKeyValue("IMACRATE");


    }   

    void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
