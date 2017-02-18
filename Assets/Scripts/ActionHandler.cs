using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
        
        var sceneItem = GetComponent<SceneItem>();
        print("Hey Yo I selected a component!");

        //sceneItem.keyValue = "juice";
        print("My name is " + ruckSack.keyValue);
        ruckSack.addTwinItem(sceneItem);
        ruckSack.keyValue = sceneItem.keyValue;

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

