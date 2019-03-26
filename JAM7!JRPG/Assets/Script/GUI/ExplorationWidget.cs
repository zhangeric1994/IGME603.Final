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

    public override void Initialize(params object[] args)
    {
        menu.Initialize(args);
    }
}
