using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class Mypointer : PointerInputModule
{
    [SerializeField]
    private RectTransform m_VirtualCursor;
    public EventSystem es;
    public Camera cam;
    private Vector2 auxVec2;
    PointerEventData pointer;
   
       
    
    public override void Process()
    {
    pointer = new PointerEventData(es);
    Vector3 screenPos = cam.WorldToScreenPoint(m_VirtualCursor.transform.position);
        
        auxVec2.x = screenPos.x;
        auxVec2.y = screenPos.y;
        Debug.Log(screenPos.x);
        Debug.Log(screenPos.y);

        pointer.position = auxVec2;
        es.RaycastAll(pointer, this.m_RaycastResultCache);
        RaycastResult raycastResult = FindFirstRaycast(this.m_RaycastResultCache);
        pointer.pointerCurrentRaycast = raycastResult;
        Debug.Log(this.m_RaycastResultCache.Count);
        Debug.Log(raycastResult.ToString());
        this.ProcessMove(pointer);
    }

   

    protected virtual void ProcessMove(PointerEventData pointerEvent)
    {
        GameObject gameObject = pointerEvent.pointerCurrentRaycast.gameObject;
        base.HandlePointerExitAndEnter(pointerEvent, gameObject);
    }



}