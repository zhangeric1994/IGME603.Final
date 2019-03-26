using UnityEngine;
using UnityEngine.UI;

public class MenuWidget : GUIWidget
{
    //[Header("References")]
    //[SerializeField] private Transform list;

    private string playerID;
    private int maxIndex;
    private int choosenIndex;

    private GUIWidget widgetOpened;

    public override void Initialize(params object[] args)
    {
        maxIndex = transform.childCount;

        playerID = ((PlayerController)args[0]).Id.ToString();
    }

    private void Update()
    {
        if (widgetOpened)
        {
            if (Input.GetButtonDown("Cancel" + playerID))
            {
                widgetOpened.GetComponent<Text>().color = Color.white;
                widgetOpened.Hide();
            }
        }
        else if (Input.GetButtonDown("Submit" + playerID))
        {
            if (Input.GetButtonDown("Up" + playerID))
                choosenIndex = --choosenIndex % maxIndex;
            else if (Input.GetButtonDown("Down" + playerID))
                choosenIndex = ++choosenIndex % maxIndex;
            else
            {
                widgetOpened = transform.GetChild(choosenIndex).GetComponent<GUIWidget>();
                widgetOpened.Initialize(playerID);
                widgetOpened.GetComponent<Text>().color = Color.red;
                widgetOpened.Show();
            }
        }
    }
}
