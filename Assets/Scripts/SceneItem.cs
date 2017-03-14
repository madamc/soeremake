using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SceneItem : MonoBehaviour {

    public String Name;
    public bool isPortal = false;
    public String Description;
  

    public Dictionary<String, SceneItem> itemSocket;  //dictionary<dictionary> itemSocket
    public String keyValue;
    internal bool isInventory;
    public bool isPickupable;
    public bool isTalkToable;
    public bool isEdible;
    public bool isEquipable;
    public bool isGiveable;
    public bool isFightable;
    public bool isLookable;
    public bool isContextMenuButton;
    public string eatMessage;
    public List<String> responses;
    public UnityAction contextButtonClickListener;
    private int responsecounter = 0;

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

    public int GetBoolCount()
    {
        int counter = 0;
        if (isPickupable) { counter++; }
        if (isEdible) { counter++; }
        if (isEquipable) { counter++; }
        if (isFightable) { counter++; }
        if (isGiveable) { counter++; }
        if (isTalkToable) { counter++; }
        if (isLookable) { counter++; }
        return counter;
      
    }

    private void Awake()
    {
        if (responses != null)
        {
            responses.Reverse();
           responsecounter = responses.Count-1;
        }
        
    }

    public void destroyListener(string key)
    {
        EventManager.StopListening(key, contextButtonClickListener);


    }

    public string callNextResponse()
    {
        if (responsecounter >= 0)
        {
            string ret = responses[responsecounter];
            responsecounter--;
            return ret;
        }
        else return ("The " + Name + " says nothing more");

    }

}
