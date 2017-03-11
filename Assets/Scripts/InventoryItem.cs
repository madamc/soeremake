using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour {

    public string name;
    public string description;
    public string keyValue;
    public Sprite sprite;
    public int id;
    public SceneItem sceneItem;

   public InventoryItem(string name, string description, string keyValue)
    {
        this.name = name;
        this.description = description;
        this.keyValue = description;

    }

  

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
