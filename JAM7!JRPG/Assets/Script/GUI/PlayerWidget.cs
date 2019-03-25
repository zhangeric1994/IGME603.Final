using UnityEngine;
using UnityEngine.UI;

public class PlayerWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private Text magazineText;
    [SerializeField] private PlayerHealthWidget healthWidget;

    public override void Redraw(params object[] args)
    {
        PlayerController player = (PlayerController)args[0];

        UpdateMagazine(player.Magazine);

        player.OnMagazineUpdate.AddListener(UpdateMagazine);

        healthWidget.Redraw(player);
    }

    private void UpdateMagazine(int n)
    {
        magazineText.text = n.ToString();
    }
}
