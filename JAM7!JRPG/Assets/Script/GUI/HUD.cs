using UnityEngine;

public class HUD : GUIWindow
{
    public static HUD Singleton { get; private set; }

    [Header("References")]
    [SerializeField] private ExplorationWidget[] playerExplorationWidgets;
    [SerializeField] private CombatWidget[] playerCombatWidgets;

    public void ShowMenu(int id)
    {
        playerExplorationWidgets[id].ShowMenu();
    }

    public void HideMenu(int id)
    {
        playerExplorationWidgets[id].HideMenu();
    }

    public void ShowExplorationUI(int id)
    {
        playerExplorationWidgets[id].Show();
    }

    public void HideExplorationUI(int id)
    {
        playerExplorationWidgets[id].Hide();
    }

    public void ShowCombatUI(int id)
    {
        playerCombatWidgets[id].Show();
    }

    public void HideCombatUI(int id)
    {
        playerCombatWidgets[id].Hide();
    }

    public override void OnOpen(params object[] args)
    {
        if (!Singleton)
            Singleton = this;
        else
        {
            Close();
            return;
        }

        for (int id = 0; id < 2; ++id)
        {
            playerExplorationWidgets[id].Initialize(id);
            playerCombatWidgets[id].Initialize(args[id]);
        }
    }

    public override void OnClose()
    {
        if (this == Singleton)
            Singleton = null;
    }
}
