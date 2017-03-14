using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GlobalControl : MonoBehaviour {
    public static GlobalControl Instance;
    public List<InventoryItem> itemsInInventory;
    public List<GameObject> listOfAllItems;
    public Dictionary<string, UnityEvent> eventDictionary;
    public float globalCameraSize=120f;
    public Dictionary<string, InventoryItem> inventoryItemDictionary;
    public Dictionary<string, bool> hasHappenedDictionary;
    void Awake()
    {
        itemsInInventory = new List<InventoryItem>();
        hasHappenedDictionary = new Dictionary<string, bool>();
        eventDictionary= new Dictionary<string, UnityEvent>(); 
        new Dictionary<string, UnityEvent>();
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
        

	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void PopulateDB()
    {
      
    }
}
