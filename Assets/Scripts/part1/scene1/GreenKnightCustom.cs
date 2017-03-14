using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GreenKnightCustom : MonoBehaviour {

    public UnityAction sadGreenKnightListener;
    void Start()
    {
        
        bool result=false;
        GlobalControl.Instance.hasHappenedDictionary.TryGetValue(this.gameObject.name, out result);
        Debug.Log(result);  
        if (!result)
        {
            gameObject.SetActive(false);
        }
    }
   
    public void activateGreenKnight() {
        
        bool result;
        GlobalControl.Instance.hasHappenedDictionary.TryGetValue(this.gameObject.name, out result);
        if (!result) {
            this.gameObject.SetActive(true);
            GlobalControl.Instance.hasHappenedDictionary.Add(this.gameObject.name, true);
                }
        gameObject.GetComponent<FadeIn>().isFadingIn = true;
        
        
        }

}
