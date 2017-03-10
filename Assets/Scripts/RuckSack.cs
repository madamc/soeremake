using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
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
    public GameObject inventorycanvas;
    public GameObject spotlight;
    public GameObject jibberJabberPanel;
    public JibberJabber jibberJabber;
    public GameObject contextPanel;
    GameObject[] golist;
    public float contextMenuOffset =50f;
    public SimpleObjectPool buttonObjectPool;
    void Start()
    {
        jibberJabber = jibberJabberPanel.GetComponent<JibberJabber>();
     

        contextPanel.SetActive(false);
        spotlight = GameObject.Find("Spotlight");
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

      
        ////todo .. prolly should move this to start and update it only when needed.
        golist= GameObject.FindGameObjectsWithTag("SelectableObject");
        if (keyValue != null) { 
        }
        Vector3 delta;

        float h = horizontalMouseSpeed * Input.GetAxis("Mouse X");
            float v = verticalMouseSpeed * Input.GetAxis("Mouse Y");

        Rect screenRect = new Rect(0, 0, screenxmax+0.1f, screenymax+0.1f);
        
        
        float currentx = mouseTransform.position.x;
        float currenty = mouseTransform.position.y;

        //        //todo Figure out how to prevent this from firing several times each time you click it.
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("Clearing item selected By Cursor");
            ClearCursor();
            

        }

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
                ClearCursor();
                //Not sure where to populate this
                if (inventorydb.itemsInInventory.Count == 0)
                {
                    inventorycanvas.SetActive(true);
                }

                foreach (InventoryItem it in inventorydb.itemsInInventory)
                {
                    GameObject go = createGoInventoryItem(it);
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
               
                
                    deselectAllItems(golist);

                

                if (selectedObj == 0)
                {
                    selectedObj = listOfSelectableGameObjects.Count - 1;
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[listOfSelectableGameObjects.Count - 1].transform.position.x,
                        listOfSelectableGameObjects[listOfSelectableGameObjects.Count - 1].transform.position.y-mouseCursor.GetComponent<BoxCollider2D>().offset.y));
             //       newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y *
               //         listOfSelectableGameObjects[listOfSelectableGameObjects.Count -1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    mouseTransform.position =newvec;
                }
                else
                {
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                    listOfSelectableGameObjects[selectedObj - 1].transform.position.x,
                    listOfSelectableGameObjects[selectedObj - 1].transform.position.y - mouseCursor.GetComponent<BoxCollider2D>().offset.y));
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

                deselectAllItems(golist);


                if (selectedObj == listOfSelectableGameObjects.Count - 1)
                {
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[0].transform.position.x,
                        listOfSelectableGameObjects[0].transform.position.y - mouseCursor.GetComponent<BoxCollider2D>().offset.y));
             //       newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y * listOfSelectableGameObjects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    

                    mouseTransform.position = newvec;
                    selectedObj = 0;
                }
                else
                {
                    Vector2 newvec= sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[selectedObj + 1].transform.position.x,
                        listOfSelectableGameObjects[selectedObj + 1].transform.position.y - mouseCursor.GetComponent<BoxCollider2D>().offset.y));
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


        //This breaks the move with keyboard thing... again.  Need to find a new offset. 
        Vector2 mousev = sceneCamera.ScreenToWorldPoint(new Vector3(mouseCursor.transform.position.x,mouseCursor.transform.position.y + mouseCursor.GetComponent<BoxCollider2D>().offset.y, mouseCursor.transform.position.z));

        //This checks to see what is overlapping with the cursor
        bool mousedOver = false;

        Collider2D[] col = Physics2D.OverlapPointAll(mousev);



        if (col.Length > 0)
        {

           Collider2D c = col[0];
            //todo:  not sure about this, ignoring any other collisions.
            //foreach (Collider2D c in col)
            //{
                if (c.gameObject.GetComponent<SceneItem>() != null)
                {
                    SceneItem sceneItem = c.gameObject.GetComponent<SceneItem>();
                    mousedOver = true;


                 
                 

                        if (Input.GetMouseButtonDown(0))
                        {
                    //hide the jibberjabber
                    if (!jibberJabber.isHid) { 
                        jibberJabber.hideme();
                    }

                    if (sceneItem.isContextMenuButton)
                        {
                            Debug.Log("fire Trigger");
                            EventManager.TriggerEvent(sceneItem.name);
                        }
                    
                    else if (!c.gameObject.name.Contains("contextPanel"))
                    {
                        //if it's not a context menu button, then hide the context panel.
                        contextPanel.SetActive(false);
                    }
                        try
                            {
                                bool itemSelected = false;
                                if (selectedItemKey != "nothing")
                                {
                                    itemSelected = true;
                                }
                                bool selectingItem = false;
                                bool isPortal;
                                bool isInventory;


                                isInventory = sceneItem.isInventory;
                                isPortal = sceneItem.isPortal;
                                //handle the action if it is NOT a portal.
                                if (!isPortal && (isInventory || itemSelected))
                                {
                                    Debug.Log("Sceneitem selected");
                                    if (cursorSocket.Count > 0)
                                    {
                                        itemSelected = true;
                                        var selectable = c.GetComponent<Selectable>();
                                        if (selectable == null) return;
                                        selectable.Select();
                                    }

                                    //sceneItem.keyValue = "juice";

                                    //Not sure if we want to worry about the rucksack's key value anymore.  
                                    //print("My name is " + ruckSack.keyValue);
                                    if (!isAlreadySelected(sceneItem) && !itemSelected)
                                    {

                                        addTwinItem(sceneItem);
                                        Destroy(sceneItem.gameObject);
                                     

                                //Todo, allow for craft, etc.
                                inventorycanvas.SetActive(false);
                                        selectingItem = true;
                                    }
                                    if (!selectingItem && itemSelected)
                                    {
                                        compareObjectAction(sceneItem);
                                    }
                                    //ruckSack.keyValue = sceneItem.keyValue;
                                }//end if portal
                                else if (!isPortal && !isInventory && !itemSelected)
                                {
                                    //todo Need to add context menu here
                                    if (sceneItem.isPickupable && sceneItem.GetBoolCount() > 1)
                                    {
                                        //If this sceneitem can be picked up, but there are more options to interact with it,
                                        //create a context menu
                                        contextCreator(sceneItem);
                                    }
                                    else
                                    if (sceneItem.isPickupable && sceneItem.GetBoolCount() == 1)
                                    {
                                        //if it is pickupable, and there are no other options to interact
                                        //just pick it up.  

                                        addToInventory(sceneItem);


                                        //Implement Wait Till animation completed

                                        //also, need to learn to recycle.  
                                        Destroy(sceneItem.gameObject);
                                       
                                        //todo:  this isn't working for some reason.  
                                        listOfSelectableGameObjects.Clear();
                                        populateListOfSelectableGameObjects(listOfSelectableGameObjects);
                                    }
                                    else if (sceneItem.GetBoolCount() > 0 && !sceneItem.isPickupable)
                                    {
                                        //If if you can't pick it up, and it isn't a portal, make a context menu based on what's left.
                                        contextCreator(sceneItem);
                                    }
                                    else
                                    {
                                        generateCannotPickupMessage(sceneItem.keyValue);
                                    }
                                }
                                else if (isPortal)
                                {
                                    //If you're a portal, do this.  
                                    Zoomer zoom = (Zoomer)(sceneCamera.GetComponent<Zoomer>());
                                    zoom.ZoomToScene(sceneItem.keyValue, (sceneItem.transform));
                                }
                            }//end try
                            catch (NullReferenceException n)
                            {
                                Debug.Log("ITEM NOT SET as a SCENEITEM");
                            }
                        }//end if
                  


                }//end if



                if (c.gameObject.GetComponent<SpriteRenderer>() != null)
                {

                    SpriteRenderer spriteRenderer = c.gameObject.GetComponent<SpriteRenderer>();
                    if (!spriteRenderer.material.name.Contains("HighlightMat"))
                    {
                        {

                            spotlight.GetComponent<Light>().enabled = true;
                            spotlight.transform.position = new Vector3(spriteRenderer.gameObject.transform.position.x, spriteRenderer.gameObject.transform.position.y, spotlight.transform.position.z);
                            spriteRenderer.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/HighlightMat.mat", typeof(Material));
                        }
                    }
                }
                else if (c.gameObject.GetComponent<Image>() != null)
                {

                    Image spriteImage = c.gameObject.GetComponent<Image>();
                    if (!spriteImage.material.name.Contains("HighlightMat"))
                    {
                        spriteImage.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/HighlightMat.mat", typeof(Material));
                    }
                }

            //}

         
            
        }//end if col length
        else 
        if (Input.anyKeyDown) {
            if (!jibberJabber.isHid) { 
                jibberJabber.hideme();
            }
            contextPanel.SetActive(false);
                }

        if (!mousedOver)
        {
            deselectAllItems(golist);
            
        }

    }

    public void contextCreator(SceneItem si)
    {
       
        if (si.isPickupable)
        {
            GameObject button = buttonObjectPool.GetObject();
            button.name = "Pick Up";
            // todo:  probably should implement some sort of string resource here:
            button.GetComponentInChildren<Text>().text = "Pick Up";

            button.transform.SetParent(contextPanel.transform, false);
            button.transform.position = new Vector3(button.transform.position.x, transform.position.y, contextPanel.transform.position.z);
            button.transform.localScale = Vector3.one;
            SceneItem it = button.GetComponent<SceneItem>();

            UnityAction pickupListener = new UnityAction(()=>PickUpObjectWithMenu(si));
            si.contextButtonClickListener = pickupListener;
            EventManager.StartListening(it.name, pickupListener);
            Debug.Log("Start Listening");
         

        }

        if (si.isLookable)
        {
            GameObject button = buttonObjectPool.GetObject();
            button.name = "Look at";
            // todo:  probably should implement some sort of string resource here:
            button.GetComponentInChildren<Text>().text = "Look at";

            button.transform.SetParent(contextPanel.transform, false);
            button.transform.position = new Vector3(button.transform.position.x, transform.position.y, contextPanel.transform.position.z);
            button.transform.localScale= Vector3.one;
            SceneItem it = button.GetComponent<SceneItem>();

            UnityAction pickupListener = new UnityAction(() => LookAtSceneItemWithMenu(si));
            si.contextButtonClickListener = pickupListener;
            EventManager.StartListening(it.name, pickupListener);
            Debug.Log("Start Listening");


        }
        contextPanel.transform.position = new Vector3(si.transform.position.x + contextMenuOffset, si.transform.position.y, 1f);

        contextPanel.SetActive(true);

    }

    public void LookAtSceneItemWithMenu(SceneItem si)
    {
        Debug.Log(si.Description);
        jibberJabber.setTextandShow(si.Description);

        //also, need to learn to recycle.  
        si.destroyListener();
        //todo:  this isn't working for some reason.  


        clearContextMenuButtons();
        contextPanel.SetActive(false);
    }

    //Return all buttons to the pool for the next context menu
    private void clearContextMenuButtons()
    {
        int count = contextPanel.transform.GetChildCount();
        for (int i=0;i<count; i++)
        {
            GameObject go = contextPanel.transform.GetChild(0).gameObject;
            buttonObjectPool.ReturnObject(go);
        }
    }

    public void PickUpObjectWithMenu(SceneItem si)
    {
        //todo Start recycling game objects, dude
        Debug.Log("Caught Listener");

        addToInventory(si);


        //Implement Wait Till animation completed
        //MUST STOP LISTENING
        //also, need to learn to recycle.  
        si.destroyListener();
        Destroy(si.gameObject);
       
        //todo:  this isn't working for some reason.  
        listOfSelectableGameObjects.Clear();
        populateListOfSelectableGameObjects(listOfSelectableGameObjects);

        clearContextMenuButtons();
        contextPanel.SetActive(false);

    }

    private void deselectAllItems(GameObject[] golist)
    {
        foreach (GameObject go in golist)
        {
            if (go.GetComponent<SpriteRenderer>() != null)
            {
                SpriteRenderer spriteRenderer = go.GetComponent<SpriteRenderer>();
                spotlight.GetComponent<Light>().enabled = false;
                spriteRenderer.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/LightSpriteMat.mat", typeof(Material));
            }
            else if (go.GetComponent<SpriteRenderer>() == null)
            {
                Image spriteImage = go.GetComponent<Image>();
                spotlight.GetComponent<Light>().enabled = false;
                spriteImage.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/PlainMat.mat", typeof(Material));
            }

        }

    }

    public void generateCannotPickupMessage(string keyValue)
    {
        switch (keyValue)
        {
            case "IMATABLE":
                Debug.Log("Thine armor is not spacious enough, m'lord.");
                break;
            default:
                Debug.Log("Ye cannot pick that up.");
                break;
        }
    }

    internal void addToInventory(SceneItem sceneItem)
    {

        Debug.Log("Adding to Inventory");

        if (inventorydb.itemsInInventory.Count == 0)
        {
            inventorydb.itemsInInventory.Add(inventorydb.inventoryItemDictionary[sceneItem.keyValue]);
        }
        else
        {
            bool notfound = true;
            foreach (InventoryItem it in inventorydb.itemsInInventory)
            {

                if ((it.keyValue == sceneItem.keyValue))
                {
                    notfound = false;
                    Debug.Log("already added to inventory");
                    break;

                }
            }
            if (notfound)
            {
                inventorydb.itemsInInventory.Add(inventorydb.inventoryItemDictionary[sceneItem.keyValue]);
            }
        }//else
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
        selectedItemKey = item.keyValue;

        Sprite tempsprite;
        if (item.GetComponent<SpriteRenderer>() != null)
        {
            tempsprite = (item.GetComponent<SpriteRenderer>()).sprite;
        }
        else
        {
            tempsprite = (item.GetComponent<Image>().sprite);
        }
        mouseImage.sprite = tempsprite;

       

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

        else if (selectedItemKey == "IMACANDLE")
        {
            switch (item.keyValue)
            {
                case "IMATABLE":
                    Debug.Log("You drop the candle, and it snuffs out. Nice going, m'lord! ");
                    removeInventoryItemWithKeyvalue(keyValue);
                    break;
                default:
                    Debug.Log("Tsk tsk... What would your mother say...");
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


    public GameObject createGoInventoryItem(InventoryItem it)
    {
        GameObject go = new GameObject();
        go.AddComponent<ActionHandler>();
        go.GetComponent<ActionHandler>().ruckSack = this;
        go.GetComponent<ActionHandler>().cursorobj = mouseCursor;
        go.AddComponent<BoxCollider2D>();
        go.GetComponent<BoxCollider2D>().size = new Vector2(20, 20);
        go.AddComponent<Image>();
        go.GetComponent<Image>().sprite = it.sprite;
        go.GetComponent<Image>().preserveAspect = true;
        go.GetComponent<ActionHandler>().spriteImage = go.GetComponent<Image>();
        go.GetComponent<ActionHandler>().spriteImage.sprite = it.sprite;
        go.name = it.name;
        
        //Add ToolTip with description
        go.AddComponent<SceneItem>();
        go.GetComponent<SceneItem>().name = it.name;
        go.GetComponent<SceneItem>().keyValue = it.keyValue;
        go.GetComponent<SceneItem>().isInventory = true;
        return go;
    }
    public void ClearCursor()
    {
        cursorSocket.Clear();
        selectedItemKey = "nothing";
        mouseCursor.GetComponent<Image>().sprite =
            Sprite.Create(NormalCursor,
            new Rect(0, 0, NormalCursor.width,
            NormalCursor.height), new Vector2(0.5f, 0.5f));
    }

    public void removeInventoryItemWithKeyvalue(string keyValue)
    {
        int elementToDestroy=0;
        for (int i=0;i < inventorydb.itemsInInventory.Count;i++)
        {
            if (selectedItemKey == keyValue)
            {
                elementToDestroy = i;
            }
        }
        //todo:  kind of worried about this implementation
       
            inventorydb.itemsInInventory.RemoveAt(elementToDestroy);

        ClearCursor();
        
    }
}
