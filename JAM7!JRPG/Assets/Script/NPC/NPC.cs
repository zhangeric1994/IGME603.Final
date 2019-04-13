using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class NPC : MonoBehaviour {
    List<string> dialogue;
    public TextMeshProUGUI textGUI;

    private int dialogueCNT;

    private bool isTalk;

    private bool isTouch; 

    public Image img;



    void Start(){
        dialogueCNT = 0;
        isTalk = false;
        isTouch = false;

        dialogue = new List<string>();
        dialogue.Add("hello");
        dialogue.Add("I'm NPC");

        img.enabled = false;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.Z) & isTouch){
            Debug.Log("Press z");
            Talk();
        }
    }

    void Talk(){
        if (dialogueCNT == 0) img.enabled = true;

        if (dialogueCNT >= dialogue.Count){
            img.enabled = false;
            textGUI.SetText("");
            dialogueCNT = 0;

            //unfreeze scene
            Time.timeScale = 1.0f;
            return;
        }

        textGUI.SetText(dialogue[dialogueCNT]);
        dialogueCNT++;

        //freeze scene
        Time.timeScale = 0;

    }

    private void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("touch the NPC");
        isTouch = true;
    }

    private void OnTriggerExit2D(Collider2D other) {
        Debug.Log("PLayer Leave");
        isTouch = false;
    }


}
