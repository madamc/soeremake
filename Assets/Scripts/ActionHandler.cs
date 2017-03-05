using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ActionHandler : Selectable
{
    public Selectable selectObject;
    public SpriteRenderer spriteRenderer;
    public Image spriteImage;
    public RuckSack ruckSack;
    Camera cam;
    public GameObject cursorobj;
    SceneItem sceneItem;


    private bool isMousedOver;

    protected override void Start()
    {
        sceneItem = GetComponent<SceneItem>();
        cursorobj = GameObject.Find("Cursor");
        cam = GameObject.Find("Main Camera").GetComponent<Camera>();

    }

    protected override void Awake()
    {
        if (ruckSack == null)
        {
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) {
            spriteImage = GetComponent<Image>();
                }
        ruckSack = GameObject.Find("RuckSack").GetComponent<RuckSack>();
        if (ruckSack != null)
        {
        }
    }



    private void Update()
    {
        if (isMousedOver && spriteRenderer != null)
        {
            spriteRenderer.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/HighlightMat.mat", typeof(Material));
        }
        else if (isMousedOver && spriteRenderer == null)
        {
            spriteImage.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/HighlightMat.mat", typeof(Material));
        }
        else if (!isMousedOver && spriteRenderer != null)
        {
            spriteRenderer.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/PlainMat.mat", typeof(Material));
        }
        else if (!isMousedOver && spriteRenderer == null)
        {
            spriteImage.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/PlainMat.mat", typeof(Material));
        }



            //todo Figure out how to prevent this from firing several times each time you click it.
            if (Input.GetMouseButtonDown(1)) {
            Debug.Log("Clearing item selected By Cursor");
            ruckSack.ClearCursor();
          
        }

        Vector2 mousev = cam.ScreenToWorldPoint(cursorobj.transform.position);
          
        //This checks to see what is overlapping with the cursor

        Collider2D[] col = Physics2D.OverlapPointAll(mousev);
        
   

        if (col.Length > 0 && sceneItem != null)
        {
            
            foreach (Collider2D c in col)
            {
                if (sceneItem.gameObject.GetInstanceID()== c.gameObject.GetInstanceID())
                {
                    isMousedOver = true;


                    if (Input.GetMouseButtonDown(0))
                    {
                        try
                        {
                            bool itemSelected = false;
                            if (ruckSack.selectedItemKey != "nothing") 
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
                                if (ruckSack.cursorSocket.Count > 0)
                                {
                                    itemSelected = true;
                                    var selectable = c.GetComponent<Selectable>();
                                    if (selectable == null) return;
                                    selectable.Select();
                                }

                                //sceneItem.keyValue = "juice";

                                //Not sure if we want to worry about the rucksack's key value anymore.  
                                //print("My name is " + ruckSack.keyValue);
                                if (!ruckSack.isAlreadySelected(sceneItem) && !itemSelected)
                                {
                                    
                                    ruckSack.addTwinItem(sceneItem);
                                    Destroy(sceneItem.gameObject);

                                    //Todo, allow for craft, etc.
                                    ruckSack.inventorycanvas.SetActive(false);
                                    selectingItem = true;
                                }
                                if (!selectingItem && itemSelected)
                                {
                                    ruckSack.compareObjectAction(sceneItem);
                                }
                                //ruckSack.keyValue = sceneItem.keyValue;
                            }//end if portal
                            else if (!isPortal && !isInventory&&!itemSelected)
                            {
                                //todo Need to add context menu here
                                if (sceneItem.isPickupable)
                                {
                               
                                    ruckSack.addToInventory(sceneItem);
                                    //Implement Wait Till animation completed
                                    Destroy(sceneItem.gameObject);
                                    
                                }
                                else
                                {
                                    ruckSack.generateCannotPickupMessage(sceneItem.keyValue);
                                }
                            } else if (isPortal)
                            {
                                //If you're a portal, do this.  
                                Zoomer zoom = (Zoomer)(cam.GetComponent<Zoomer>());
                                zoom.ZoomToScene(sceneItem.keyValue, (sceneItem.transform));
                            }
                        }//end try
                        catch (NullReferenceException n)
                        {
                            Debug.Log("ITEM NOT SET as a SCENEITEM");
                        }
                    }//end if


                }//end if



                else
                {
                    isMousedOver = false;
                }
            }
        }
        else
        {
            isMousedOver = false;
        }


    }//end update 

 

}

