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
    private float screenxmax;
    public Camera sceneCamera;
    private int selectedObj=0;
    public List<GameObject> listOfSelectableGameObjects;
    private bool _inputDelayOn = false;
    private float _inputDelayTimer=0.0f;
    public float inputDelayTime = 0.5f;
    void Start()
    {
        listOfSelectableGameObjects = new List<GameObject>();
        populateListOfSelectableGameObjects(listOfSelectableGameObjects);
        cursorCanvas = GameObject.Find("CursorCanvas");
        mouseCanvas = cursorCanvas.GetComponent<Canvas>();
        mouseImage = mouseCanvas.GetComponentInChildren<Image>();
        sceneCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
       

        screenymax = Screen.height * 0.95f;
                      
        screenxmax = Screen.width * 0.99f;
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

            mouseTransform.position += delta;
         
            //         mouseTransform.position += delta; // moves the virtual cursor
            // You need to clamp the position to be inside your wanted area here,
            // otherwise the cursor can go way off screen
        }

        bool leftdirection = Input.GetButtonDown("LeftBumper");
        bool rightdirection = Input.GetButtonDown("RightBumper");

        if (_inputDelayOn) {

            _inputDelayTimer += Time.deltaTime;
            if (_inputDelayTimer >= inputDelayTime) {
                _inputDelayOn = false;
                _inputDelayTimer = 0.0f;
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
                    newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y *
                        listOfSelectableGameObjects[listOfSelectableGameObjects.Count -1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    mouseTransform.position =newvec;
                }
                else
                {
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                    listOfSelectableGameObjects[selectedObj - 1].transform.position.x,
                    listOfSelectableGameObjects[selectedObj - 1].transform.position.y));
                    newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y *
                       listOfSelectableGameObjects[selectedObj-1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
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
                    Debug.Log("how come this isn't sotpping this train?");
                    return;
                }
                Debug.Log(_inputDelayOn);
                Debug.Log("howManyRight");
                if (selectedObj == listOfSelectableGameObjects.Count - 1)
                {
                    Vector2 newvec = sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[0].transform.position.x,
                        listOfSelectableGameObjects[0].transform.position.y));
                    newvec.y = newvec.y + listOfSelectableGameObjects[0].transform.localScale.y * listOfSelectableGameObjects[0].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
                    

                    mouseTransform.position = newvec;
                    selectedObj = 0;
                }
                else
                {
                    Vector2 newvec= sceneCamera.WorldToScreenPoint(new Vector2(
                        listOfSelectableGameObjects[selectedObj + 1].transform.position.x,
                        listOfSelectableGameObjects[selectedObj + 1].transform.position.y));
                    newvec.y = newvec.y + listOfSelectableGameObjects[selectedObj + 1].transform.localScale.y 
                        * listOfSelectableGameObjects[selectedObj + 1].GetComponent<SpriteRenderer>().sprite.bounds.size.y;
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
        selectedItemKey = item.keyValue;
       Sprite tempsprite= (item.GetComponent<SpriteRenderer>()).sprite;
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
