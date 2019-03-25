using UnityEngine;

public class HUD : GUIWindow
{
    [Header("References")]
    [SerializeField] private CombatWidget player0Widget;
    [SerializeField] private CombatWidget player1Widget;

    public override void OnOpen(params object[] args)
    {
        PlayerController player0 = (PlayerController)args[0];
        PlayerController player1 = (PlayerController)args[1];

        player0Widget.Initialize(player0);
        player0Widget.Initialize(player1);
    }
}
