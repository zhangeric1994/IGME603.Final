using UnityEngine;
using UnityEngine.UI;

public class CombatWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private Text magazineText;
    [SerializeField] private PlayerHealthWidget healthWidget;

    public override void Initialize(params object[] args)
    {
        PlayerController player = (PlayerController)args[0];

        UpdateMagazine(player.Magazine);

        player.OnMagazineUpdate.AddListener(UpdateMagazine);

        healthWidget.Initialize(player);
    }

    private void UpdateMagazine(int n)
    {
        magazineText.text = n.ToString();
    }
}
