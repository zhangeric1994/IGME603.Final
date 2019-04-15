using UnityEngine;

public class ExplorationWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private MenuWidget menu;

    public void ShowMenu()
    {
        menu.Show();
    }

    public void HideMenu()
    {
        menu.Hide();
    }

    public override void Refresh(params object[] args)
    {
        menu.Refresh(args);
    }
}
