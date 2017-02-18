using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuckSack : MonoBehaviour {

    public String name;
    public String description;

    public Dictionary<String, SceneItem> cursorSocket;  //dictionary<dictionary> itemSocket
    public String keyValue;

    
	// Use this for initialization
    //Initialize the curser's pocket
	void Awake() {
        //print("This getting caleed!");
        cursorSocket = new Dictionary<string, SceneItem>();
    }
	
	// Update is called once per frame
	void Update () {
        if (keyValue != null) { 
        //print(keyValue);
        }
    }

    public void addTwinItem(SceneItem item)
    {
        // Check the value of the item to see if it's the right \\corresponding value     
        cursorSocket.Add(item.keyValue, item);
        print("My name is " + cursorSocket.Values);
    }
    
}
