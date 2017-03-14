using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LoadSceneEvents : MonoBehaviour {

	// Use this for initialization
	void Awake () {
        //this will load events and listeners based on the scene name  
            switch (gameObject.scene.name)
            {
                case "part1_scene1":
                GreenKnightCustom go=GameObject.Find("GreenKnight").GetComponent<GreenKnightCustom>();
                UnityAction sadGreenKnightListener = new UnityAction(() => go.activateGreenKnight());

                go.sadGreenKnightListener = sadGreenKnightListener;
                EventManager.StartListening("Eatpart1_scene1_moldybread", sadGreenKnightListener);

                break;
                default:
                    
                    break;
            }

    }
	
}
