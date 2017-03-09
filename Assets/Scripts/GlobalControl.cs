using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalControl : MonoBehaviour {
    public static GlobalControl Instance;
    public List<InventoryItem> itemsInInventory;
    public List<GameObject> listOfAllItems;
    public float globalCameraSize=120f;
    public Dictionary<string, InventoryItem> inventoryItemDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    // Use this for initialization
    void Start () {
        itemsInInventory = new List<InventoryItem>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PopulateDB()
    {
      
    }
}
