using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RuckSack : MonoBehaviour {

    public String name;
    public String description;
    public String selectedItemKey;
    public Texture2D NormalCursor;

    public Dictionary<String, SceneItem> cursorSocket;  //dictionary<dictionary> itemSocket
    public String keyValue;
    public GameObject cursorCanvas;
    public GameObject mouseCursor;
    private RectTransform mouseTransform;
    public float horizontalMouseSpeed=10f;
    public float verticalMouseSpeed = 10f;
    private bool canCursorMove = true;
    private Canvas mouseCanvas;
    private Image mouseImage;
    private float screenymax;
    public InventoryDB inventorydb;
    private float screenxmax;
    public GameObject inventoryPockets;
    public Camera sceneCamera;
    private int selectedObj=0;
    public List<GameObject> listOfSelectableGameObjects;
    private bool _inputDelayOn = false;
    private float _inputDelayTimer=0.0f;
    public float inputDelayTime = 0.5f;
    GameObject inventorycanvas;
    void Start()
    {

        sceneCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        screenymax = Screen.height * 0.95f;

        screenxmax = Screen.width * 0.99f;
        //hide the inventory
        inventorycanvas = GameObject.Find("InventoryCanvas");
        inventorydb = ScriptableObject.CreateInstance<InventoryDB>();
        inventorydb.PopulateDB();
        inventoryPockets = GameObject.Find("InventoryPockets");

        inventorydb.initialize();
        //RectTransform invRectTransform=inventorycanvas.GetComponent<RectTransform>();
        //invRectTransform.localScale = new Vector2(2, 2);
  
     //   invRectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);

        //   invRectTransform.position = new Vector2(0 - (Screen.width/2), -1 * Screen.height / 2);
        inventorycanvas.SetActive(false);

        listOfSelectableGameObjects = new List<GameObject>();
        populateListOfSelectableGameObjects(listOfSelectableGameObjects);
        cursorCanvas = GameObject.Find("CursorCanvas");
        mouseCanvas = cursorCanvas.GetComponent<Canvas>();
        mouseImage = mouseCanvas.GetComponentInChildren<Image>();
        
      
        mouseTransform = mouseImage.GetComponent<RectTransform>();
        Cursor.visible = false;
    }
    
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
        Vector3 delta;

        float h = horizontalMouseSpeed * Input.GetAxis("Mouse X");
            float v = verticalMouseSpeed * Input.GetAxis("Mouse Y");

        Rect screenRect = new Rect(0, 0, screenxmax+0.1f, screenymax+0.1f);
        
        
        float currentx = mouseTransform.position.x;
        float currenty = mouseTransform.position.y;

        delta = new Vector3(h, v, 0);
        if (canCursorMove)
        {

            if (!screenRect.Contains(mouseTransform.position))
            {
                if (currentx < 0)
                {
                    mouseTransform.position = new Vector3(0, mouseTransform.position.y, mouseTransform.position.z);
                }
                if (currentx >= screenxmax)
                {
                    mouseTransform.position = new Vector3(screenxmax, mouseTransform.position.y, mouseTransform.position.z);
                }
                if (currenty < 0)
                {
                    mouseTransform.position = new Vector3(mouseTransform.position.x, 0, mouseTransform.position.z);
                }
                if (currenty >= screenymax)
                {
                    mouseTransform.position = new Vector3(mouseTransform.position.x, screenymax, mouseTransform.position.z);
                }
                return;
            }
            //todo make it so if it will take it out of bounds to set it to the bound itself.  
            mouseTransform.position += delta;
         
            //         mouseTransform.position += delta; // moves the virtual cursor
            // You need to clamp the position to be inside your wanted area here,
            // otherwise the cursor can go way off screen
        }

        bool inventory = Input.GetButtonDown("Inventory");

        bool leftdirection = Input.GetButtonDown("LeftBumper");
        bool rightdirection = Input.GetButtonDown("RightBumper");

        if (_inputDelayOn) {

            _inputDelayTimer += Time.deltaTime;
            if (_inputDelayTimer >= inputDelayTime) {
                _inputDelayOn = false;
                _inputDelayTimer = 0.0f;
            }
        }


        if (inventory) {
            if (inventorycanvas.activeSelf)
            {
                for (int i=0;i < inventoryPockets.transform.childCount;i++) {
                    Transform go = (inventoryPockets.transform.GetChild(i));

                    Destroy(go.gameObject);

                }
                inventoryPockets.transform.DetachChildren();
                inventorycanvas.SetActive(false);
               
            }
            else {

                //Not sure where to populate this
                if (inventorydb.itemsInInventory.Count == 0)
                {
                    inventorycanvas.SetActive(true);
                }

                foreach (InventoryItem it in inventorydb.itemsInInventory)
                {
                    GameObject go = new GameObject();
                    go.AddComponent<ActionHandler>();
                    go.GetComponent<ActionHandler>().ruckSack = this;
                    go.GetComponent<ActionHandler>().cursorobj = mouseCursor;
                    go.AddComponent<BoxCollider2D>();
                    go.GetComponent<BoxCollider2D>().size = new Vector2(20,20);
                    go.AddComponent<Image>();
                    go.GetComponent<Image>().sprite = it.sprite;
                    go.GetComponent<Image>().preserveAspect = true;
                    go.GetComponent<ActionHandler>().spriteImage = go.GetComponent<Image>();
                    go.GetComponent<ActionHandler>().spriteImage.sprite = it.sprite;
                    go.name = it.name;
                    //Add ToolTip with description
                    go.AddComponent<SceneItem>();
                    go.GetComponent<SceneItem>().name=it.name;
                    go.GetComponent<SceneItem>().keyValue = keyValue;
                    go.GetComponent<SceneItem>().isInventory = true;
                    go.transform.SetParent(inventoryPockets.transform);
                   
                    //todo Why is this magic number needed???
                    go.GetComponent<RectTransform>().anchoredPosition3D= new Vector3(0,0,-45);

                    go.transform.localScale = Vector3.one;  
                    inventorycanvas.SetActive(true);
                }
            }
        }

        if (!_inputDelayOn) {
           
            int count = listOfSelectableGameObjects.Count;
            for (int i = 0; i < listOfSelectableGameObjects.Count; i++)
            {
                if (listOfSelectableGameObjects[i].GetInstanceID() == this.gameObject.GetInstanceID())
                {
                    selectedObj= i;
              
                }
            }

            //on left bumper or q key, this will cycle through the listOfSelectableGameObjects list, which is
            //populated at runtime with all objects tagged "selectable" on the screen.  They are sorted by x position
            //to create a logical order for the player.  
            if (leftdirection )
            {
                if (_inputDelayOn) return;
                Debug.Log("howManyLeft");
                if (selectedObj == 0)
                {
                    selectedObj = listOfSelectableGameObjects.Count - 1;
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[listOfSelectableGameObjects.Count - 1].transform.position.x,
                        listOfSelectableGameObjects[listOfSelectableGameObjects.Count - 1].transform.position.y));
             //       newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y *
               //         listOfSelectableGameObjects[listOfSelectableGameObjects.Count -1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    mouseTransform.position =newvec;
                }
                else
                {
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                    listOfSelectableGameObjects[selectedObj - 1].transform.position.x,
                    listOfSelectableGameObjects[selectedObj - 1].transform.position.y));
             //       newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y *
             //          listOfSelectableGameObjects[selectedObj-1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                       mouseTransform.position = newvec;
                    selectedObj--;
            
                }
                _inputDelayOn = true;
                return;
            }

            if (rightdirection)
            {
                if (this._inputDelayOn)
                {
                    return;
                }
                if (selectedObj == listOfSelectableGameObjects.Count - 1)
                {
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[0].transform.position.x,
                        listOfSelectableGameObjects[0].transform.position.y));
             //       newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y * listOfSelectableGameObjects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    

                    mouseTransform.position = newvec;
                    selectedObj = 0;
                }
                else
                {
                    Vector2 newvec= sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[selectedObj + 1].transform.position.x,
                        listOfSelectableGameObjects[selectedObj + 1].transform.position.y));
              //      newvec.y = newvec.y + listOfSelectableGameObjects[selectedObj + 1].transform.localScale.y 
              //          * listOfSelectableGameObjects[selectedObj + 1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    mouseTransform.position = newvec;
                    selectedObj++;
                    
                }
               this._inputDelayOn = true;
                return;
            }

        }//input delay

        if (!leftdirection && !rightdirection)
        {
            _inputDelayOn = false;
        }
        



    }
    private int SortByPositionX(GameObject g1, GameObject g2)
    {
        return g1.transform.position.x.CompareTo(g2.transform.position.x);
    }

    private void populateListOfSelectableGameObjects(List<GameObject> goList)
    {
      goList=new List<GameObject>( GameObject.FindGameObjectsWithTag("SelectableObject"));
        goList.Sort(SortByPositionX);
        this.listOfSelectableGameObjects = goList;

    }

    public void addTwinItem(SceneItem item)
    {
        // Check the value of the item to see if it's the right \\corresponding value     
        cursorSocket.Add(item.keyValue, item);
        
        
        //don't delete
        //this is the code that makes the item selected, and changes the sprite of the cursor to match.    
        //selectedItemKey = item.keyValue;

        //Sprite tempsprite;
        //if (item.GetComponent<SpriteRenderer>() != null) { 
        //tempsprite= (item.GetComponent<SpriteRenderer>()).sprite;
        //}
        //else
        //{
        //    tempsprite = (item.GetComponent<Image>().sprite);
        //}
        //mouseImage.sprite = tempsprite;

        Debug.Log("Adding to Inventory");

        if (inventorydb.itemsInInventory.Count == 0)
        {
            inventorydb.itemsInInventory.Add(inventorydb.inventoryItemDictionary[item.keyValue]);
        }
        else {
            bool notfound = true; 
            foreach (InventoryItem it in inventorydb.itemsInInventory)
            {

                if ((it.keyValue == item.keyValue))
                {
                    notfound = false;
                    Debug.Log("already added to inventory");
                    break;
                   
                }
            }
            if (notfound)
            {
                inventorydb.itemsInInventory.Add(inventorydb.inventoryItemDictionary[item.keyValue]);
            }
        }//else
       

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


    //todo might not need this
    //Makes a texture from a sprite,  this is used so we can make cursors out of the objects.  
    public static Texture2D textureFromSprite(Sprite sprite)
    {
        if (sprite.rect.width != sprite.texture.width)
        {
            Texture2D newText = new Texture2D((int)sprite.rect.width, (int)sprite.rect.height);
            Color[] newColors = sprite.texture.GetPixels((int)sprite.textureRect.x,
                                                         (int)sprite.textureRect.y,
                                                         (int)sprite.textureRect.width,
                                                         (int)sprite.textureRect.height);
            newText.SetPixels(newColors);
            newText.Apply();
            return newText;
        }
        else
            return sprite.texture;
    }

}
