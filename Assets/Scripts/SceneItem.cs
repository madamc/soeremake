using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneItem : MonoBehaviour {

    public String Name;
    public String Description;

    public Dictionary<String, SceneItem> itemSocket;  //dictionary<dictionary> itemSocket
    public String keyValue;


    public void GiveKeyValue(String givenKeyValue)
    {
        keyValue = givenKeyValue;
    }

    bool addTwinItem(SceneItem Item)
    {
		
		// Check the value of the item to see if it's the right \\corresponding value

        if (Item.keyValue == keyValue)
        {
            itemSocket.Add(Item.keyValue, Item);

        }
        else
        {
            return false;

        }

        return true; //check tutorial for code logic that checks for \\success or failure on an if statement

    }

    private void Awake()
    {
        
    }

}
