using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Zoomer : MonoBehaviour {

    public Camera sceneCamera;
    Transform target;

    private bool zoomTime = false;

    public float zoomSpeed = 1;
    public float targetOrtho;
    public float smoothSpeed = 2.0f;
    public float minOrtho = 1.0f;
    public float maxOrtho = 10.0f;
    public float yoffset = 0.0f;
    public float xoffset = 0.0f;
    private string sceneToTransitionTo;


    //#pragma 

    // FadeInOut

    public Texture2D fadeTexture;
    public float fadeSpeed = 0.2f;
    public int drawDepth = -1000;

    private float alpha = 1.0f;
    private int fadeDir = -1;
    private bool isFadingOut = false;
    private bool isFadingIn = true;
   
   




void Start()
    {
        targetOrtho = sceneCamera.orthographicSize;
    }


    void Update()
    {
        if (zoomTime)
        {
            targetOrtho -= zoomSpeed;
            targetOrtho = Mathf.Clamp(targetOrtho, minOrtho, maxOrtho);

            sceneCamera.transform.position = new Vector3((Mathf.MoveTowards(sceneCamera.transform.position.x, target.position.x+xoffset, smoothSpeed * Time.deltaTime)), (Mathf.MoveTowards(sceneCamera.transform.position.y, target.position.y+yoffset, smoothSpeed * Time.deltaTime)), sceneCamera.transform.position.z); ;

            sceneCamera.orthographicSize = Mathf.MoveTowards(sceneCamera.orthographicSize, targetOrtho, smoothSpeed * Time.deltaTime);
            
        }

        if (alpha == 1.0f && !isFadingIn)
        {
            SceneManager.LoadScene(sceneToTransitionTo);
        }

      


    }

   void OnGUI()
    {
        if (isFadingOut)
        {
            alpha -= fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color thisAlpha = GUI.color;
            thisAlpha.a = alpha;
            GUI.color = thisAlpha;

            GUI.depth = drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
        }
        if (isFadingIn) { 
            
            alpha += fadeDir * fadeSpeed * Time.deltaTime;
            alpha = Mathf.Clamp01(alpha);

            Color thisAlpha = GUI.color;
            thisAlpha.a = alpha;
            GUI.color = thisAlpha;

            GUI.depth = drawDepth;

            GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
            if (alpha == 0f)
            {
                isFadingIn= false;
            }
        }
    }

    public void ZoomToScene(string scenename, Transform pos)
    {
        target = pos;
        zoomTime = true;
        isFadingIn = false;
        isFadingOut = true;
        sceneToTransitionTo = scenename;


    }
   
   










}

