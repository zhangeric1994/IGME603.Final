﻿using UnityEngine;
using UnityEngine.UI;

public class MenuWidget : GUIWidget
{
    //[Header("References")]
    //[SerializeField] private Transform list;

    private string playerID;
    private int maxIndex;
    private int choosenIndex;

    private GUIWidget widgetOpened;

    private int ChoosenIndex
    {
        set
        {
            if (value != choosenIndex)
            {
                transform.GetChild(choosenIndex).GetComponent<Text>().color = Color.white;

                choosenIndex = value;

                transform.GetChild(choosenIndex).GetComponent<Text>().color = Color.red;
            }
        }
    }

    public override void Refresh(params object[] args)
    {
        int id = (int)args[0];

        playerID = id.ToString();

        for (int i = 0; i < transform.childCount; ++i)
            transform.GetChild(i).GetComponent<GUIWidget>().Refresh(id);
    }

    public override void Show()
    {
        base.Show();

        maxIndex = transform.childCount;
        ChoosenIndex = 0;
    }

    private void Update()
    {
        if (widgetOpened)
        {
            if (Input.GetButtonDown("Cancel" + playerID))
                widgetOpened.Hide();
        }
        else if (Input.GetButtonDown("Submit" + playerID))
        {
            widgetOpened = transform.GetChild(choosenIndex).GetComponent<GUIWidget>();
            widgetOpened.Show();
        }
        else if (Input.GetButtonDown("Up" + playerID))
            ChoosenIndex = (choosenIndex - 1) % maxIndex;
        else if (Input.GetButtonDown("Down" + playerID))
            ChoosenIndex = (choosenIndex + 1) % maxIndex;
    }
}
