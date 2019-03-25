using UnityEngine;

public class MenuWidget : GUIWidget
{
    //[Header("References")]
    //[SerializeField] private Transform list;

    PlayerController player;

    private int maxIndex;
    private int choosenIndex;

    private GUIWidget widgetOpened;

    public override void Initialize(params object[] args)
    {
        player = (PlayerController)args[0];

        maxIndex = transform.childCount;
    }

    private void Update()
    {
        string playerID = player ? player.Id.ToString() : "0";

        if (Input.GetButtonDown("Up" + playerID))
            choosenIndex = --choosenIndex % maxIndex;
        else if (Input.GetButtonDown("Down" + playerID))
            choosenIndex = ++choosenIndex % maxIndex;
        else
        {
            if (widgetOpened)
            {
                if (Input.GetButtonDown("Cancel" + playerID))
                    widgetOpened.Hide();
            }
            else if (Input.GetButtonDown("Submit" + playerID))
            {
                widgetOpened = transform.GetChild(choosenIndex).GetComponent<GUIWidget>();
                widgetOpened.Initialize(player);
                widgetOpened.Show();
            }
        }
    }
}
