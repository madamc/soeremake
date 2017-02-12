using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class ActionHandler : Selectable
{
    public Selectable selectObject;
    public SpriteRenderer spriteRenderer;

    private bool isMousedOver;

    protected override void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void OnSelect()
    {
        
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
        print("Iss do something!");
    }

    private void OnMouseExit()
    {
        isMousedOver = false;
    }
}

