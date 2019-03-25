using UnityEngine;

public class HUD : GUIWindow
{
    [Header("References")]
    [SerializeField] private PlayerWidget player0Widget;
    [SerializeField] private PlayerWidget player1Widget;

    public override void OnOpen(params object[] args)
    {
        PlayerController player0 = (PlayerController)args[0];
        PlayerController player1 = (PlayerController)args[1];

        player0Widget.Redraw(player0);
        player0Widget.Redraw(player1);
    }
}
