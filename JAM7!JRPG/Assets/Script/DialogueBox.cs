using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBox : MonoBehaviour
{
    static public DialogueBox instance;
    public Text TextObject;
    public string Text
    {
        set
        {
            TextObject.text = value;
        }
    }

    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show()
    {
        TextObject.gameObject.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        TextObject.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
