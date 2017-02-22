using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ActionHandler : Selectable
{
    public Selectable selectObject;
    public SpriteRenderer spriteRenderer;
    public RuckSack ruckSack;

    private bool isMousedOver;

    protected override void Start()
    {
       

    }

    protected override void Awake()
    {
        if (ruckSack == null)
        {
            print("It's no use, punk");
        }
        spriteRenderer = GetComponent<SpriteRenderer>();
        ruckSack = GameObject.Find("RuckSack").GetComponent<RuckSack>();
        if (ruckSack != null)
        {
            print("It's no use, it's null");
        }
    }


    public void OnMouseDown()
    {
        
        try {
            bool itemSelected = false;
            bool selectingItem = false;
            var sceneItem = GetComponent<SceneItem>();

            print("Hey Yo I selected a component!");
            if (ruckSack.cursorSocket.Count > 0)
            {
                itemSelected = true;
            }

            //sceneItem.keyValue = "juice";

            //Not sure if we want to worry about the rucksack's key value anymore.  
            //print("My name is " + ruckSack.keyValue);
            if (!ruckSack.isAlreadySelected(sceneItem) && !itemSelected)
            {
                ruckSack.addTwinItem(sceneItem);
                selectingItem = true;
            }
            if (!selectingItem && itemSelected)
            {
                ruckSack.compareObjectAction(sceneItem);
            }
            //ruckSack.keyValue = sceneItem.keyValue;
        }
        catch (NullReferenceException n)
        {
            Debug.Log("ITEM NOT SET as a SCENEITEM");
        }
      

    }

    private void Update()
    {
        if (isMousedOver)
        {
            spriteRenderer.material = (Material) AssetDatabase.LoadAssetAtPath("Assets/Materials/HighlightMat.mat", typeof(Material));
        } else if (!isMousedOver)
        {
            spriteRenderer.material = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/PlainMat.mat", typeof(Material));
        }


        //todo Figure out how to prevent this from firing several times each time you click it.
        if (Input.GetMouseButtonDown(1)) {
            Debug.Log("Clearing item selected By Cursor");
            ruckSack.cursorSocket.Clear();
            ruckSack.selectedItemKey = "nothing";
            Cursor.SetCursor(ruckSack.NormalCursor, Vector2.zero, CursorMode.Auto);
        }

            

    }

    public void OnMouseOver()
    {
        isMousedOver = true;
    }

    private void OnMouseExit()
    {
        isMousedOver = false;
    }
}

