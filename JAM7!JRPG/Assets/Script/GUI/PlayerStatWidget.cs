using UnityEngine;
using UnityEngine.UI;

public class PlayerStatWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private GameObject levelUpButton;
    [SerializeField] private GameObject promotionButtons;
    [SerializeField] private Text levelText;
    [SerializeField] private Text maxHpText;

    private bool canPlayerLevelUp;

    public override void Initialize(params object[] args)
    {
        int id = int.Parse((string)args[0]);

        Player player = Player.GetPlayer(id);

        UpdateAll(player);

        player.OnExpChange.AddListener(UpdateLevelUpButton);
        player.OnLevelChange.AddListener(UpdateLevel);
        player.OnPromotionChange.AddListener(UpdatePromotionWidget);
        player.OnMaxHpChange.AddListener(UpdateMaxHp);
    }

    private void UpdateAll(Player player)
    {
        UpdateLevelUpButton(player.CanLevelUp());
        UpdateLevel(player.Level);
        UpdateMaxHp(player.MaxHp);
    }

    private void UpdateLevelUpButton(int exp, int neededExp)
    {
        levelUpButton.SetActive(exp >= neededExp);
    }

    private void UpdateLevelUpButton(bool canLevelUp)
    {
        levelUpButton.SetActive(canLevelUp);
    }

    private void UpdatePromotionWidget(int promotion)
    {
        promotionButtons.SetActive(promotion > 0);
    }

    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }

    private void UpdateMaxHp(int maxHp)
    {
        maxHpText.text = maxHp.ToString();
    }
}
