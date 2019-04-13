using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeTest : MonoBehaviour {
    public GameData data;

    private SaveSystem saveSystem;

    void Start(){
        //saveSystem.SetData(data);
        saveSystem = SaveSystem.getInstance();
    }

    void Update(){

        if (saveSystem != null) {
            data = saveSystem.GetData();
            //Debug.Log("load");
        }
        if (saveSystem != null) {
            saveSystem.SetData(data);
            //Debug.Log("Save");
        }
        //saveSystem.SetData(data);
        if (data.isBlue) { 
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.blue); 
            //Debug.Log("Is Blue");
        }
        else {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", Color.red); 
            //Debug.Log("Not blue");
        }
    }

    
}
