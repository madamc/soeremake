using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryDB : ScriptableObject {
    public List<InventoryItem> itemsInInventory;
    public Dictionary<string, InventoryItem> inventoryItemDictionary;



	// Use this for initialization
	void Start () {
        
       
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void initialize()
    {
        this.itemsInInventory = GlobalControl.Instance.itemsInInventory;

        
    }
    

    public void PopulateDB()
    {
        inventoryItemDictionary = new Dictionary<string, InventoryItem>();
        
        foreach (GameObject go in GlobalControl.Instance.listOfAllItems){
            if (go != null)
            {
                InventoryItem it = go.GetComponent<InventoryItem>();
                it.id = it.GetInstanceID();
                inventoryItemDictionary.Add(it.keyValue, it);
            }
        }

    }
}
