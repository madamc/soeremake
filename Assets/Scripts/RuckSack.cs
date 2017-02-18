using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuckSack : MonoBehaviour {

    public String name;
    public String description;
    public String selectedItemKey;

    public Dictionary<String, SceneItem> cursorSocket;  //dictionary<dictionary> itemSocket
    public String keyValue;

    
	// Use this for initialization
    //Initialize the curser's pocket
	void Awake() {
        cursorSocket = new Dictionary<string, SceneItem>();
        selectedItemKey = "nothing";
    }
	
	// Update is called once per frame
	void Update () {
        if (keyValue != null) { 
        }
    }

    public void addTwinItem(SceneItem item)
    {
        // Check the value of the item to see if it's the right \\corresponding value     
        cursorSocket.Add(item.keyValue, item);
        selectedItemKey = item.keyValue;
        Debug.Log(item.keyValue);
    }


    //This method handles the compare operation when you use an object on another.  
    public void compareObjectAction(SceneItem item) {

        //Now for the massive list of scenarios by object type

        //  couch
        if (selectedItemKey== "IMACOUCH")
        {
            switch (item.keyValue)
            {
                case "IMACHAIR":
                    Debug.Log("Couch-a-Chair, the ultimate in comfort");
                    break;
            default:
                    Debug.Log("I don't know what you wanna do with that couch, man");
                    break;
            }
        }

        //chair
        else if (selectedItemKey == "IMACHAIR")
        {
            switch (item.keyValue)
            {
                case "IMACOUCH":
                    Debug.Log("I don't think it'd  be safe to put the chair on the couch.");
                    break;
                default:
                    Debug.Log("That's not the intended use for a chair");
                    break;
            }
        }


        //Catch all Debug statement to be able to find items with no case statements.  
        else {
            Debug.Log("Okay, fine, I admit it, you haven't told me how to handle this object, yet");
        }

        //item2
        //item3

    }

    public bool isAlreadySelected(SceneItem item)
    {
        if (!cursorSocket.ContainsKey(item.keyValue))
        {
            Debug.Log("whatcha got now?");
            return false;
           
        }
        else
        {
            Debug.Log("already selected, mate");
            return true;
           
        }
    }

}
