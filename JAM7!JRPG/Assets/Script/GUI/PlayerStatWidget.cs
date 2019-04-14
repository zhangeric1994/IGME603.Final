using UnityEngine;
using UnityEngine.UI;

public class PlayerStatWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private GameObject levelUpButton;
    [SerializeField] private GameObject promotionButtons;
    [SerializeField] private Text levelText;
    [SerializeField] private Text maxHpText;

    private Player player;

    public override void Initialize(params object[] args)
    {
        int id = (int)args[0];

        player = Player.GetPlayer(id);
    }

    public override void Show()
    {
        base.Show();

        UpdateAll();

        player.OnExpChange.AddListener(UpdateLevelUpButton);
        player.OnLevelChange.AddListener(UpdateLevel);
        player.OnPromotionChange.AddListener(UpdatePromotionWidget);
        player.OnMaxHpChange.AddListener(UpdateMaxHp);
    }

    public override void Hide()
    {
        base.Hide();

        player.OnExpChange.RemoveListener(UpdateLevelUpButton);
        player.OnLevelChange.RemoveListener(UpdateLevel);
        player.OnPromotionChange.RemoveListener(UpdatePromotionWidget);
        player.OnMaxHpChange.RemoveListener(UpdateMaxHp);
    }

    private void UpdateAll()
    {
        UpdateLevelUpButton(player.CanLevelUp());
        UpdateLevel(player.Level);
        UpdateMaxHp(0, player.GetStatistic(StatisticType.MaxHp));
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

    private void UpdateMaxHp(float previousValue, float currentValue)
    {
        maxHpText.text = ((int)currentValue).ToString();
    }

    private void OnDestroy()
    {
        player.OnExpChange.RemoveListener(UpdateLevelUpButton);
        player.OnLevelChange.RemoveListener(UpdateLevel);
        player.OnPromotionChange.RemoveListener(UpdatePromotionWidget);
        player.OnMaxHpChange.RemoveListener(UpdateMaxHp);
    }
}
