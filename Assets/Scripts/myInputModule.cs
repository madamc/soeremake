using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class myInputModule : PointerInputModule
{
    public RectTransform m_VirtualCursor;
    public Camera cam;


    public override void Process()
    {
        Vector2 v = cam.ScreenToWorldPoint(m_VirtualCursor.transform.position);
        Collider2D[] col = Physics2D.OverlapPointAll(v);

        if (col.Length > 0)
        {
            foreach (Collider2D c in col)
            {
                //Debug.Log("Collided with: " + c.collider2D.gameObject.name);
                 Debug.Log(c.GetComponent<Collider2D>().gameObject.transform.position.x);
                Debug.Log(c.GetComponent<Collider2D>().gameObject.transform.position.y);
            }
        }
    }
}