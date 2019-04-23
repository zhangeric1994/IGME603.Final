using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : GUIWindow
{
    [Header("References")]
    //[SerializeField] private GameObject levelUpButton;
    //[SerializeField] private GameObject promotionButtons;
    //[SerializeField] private Text levelText;
    [SerializeField] private Text hpText;
    //[SerializeField] private Text maxHpText;
    [SerializeField] private Text baseDamageText;
    [SerializeField] private Text attackSpeedText;
    [SerializeField] private Text criticalChanceText;
    [SerializeField] private Text criticalDamageText;
    [SerializeField] private Text walkSpeedText;
    [SerializeField] private Text jumpPowerText;

    private Action closeCallback;
    private Player player;

    public override void OnOpen(params object[] args)
    {
        closeCallback = (Action)args[0];

        player = Player.GetPlayer(0);

        UpdateAll();

        //player.OnExpChange.AddListener(UpdateLevelUpButton);
        //player.OnLevelChange.AddListener(UpdateLevel);
        //player.OnPromotionChange.AddListener(UpdatePromotionWidget);
        player.OnHpChange.AddListener(UpdateHp);
        player.OnBaseDamageChange.AddListener(UpdateBaseDamage);
        player.OnAttackSpeedChange.AddListener(UpdateAttackSpeed);
        player.OnCriticalChanceChange.AddListener(UpdateCriticalChance);
        player.OnCriticalDamageChange.AddListener(UpdateCriticalDamage);
        player.OnWalkSpeedChange.AddListener(UpdateWalkSpeed);
        player.OnJumpPowerChange.AddListener(UpdateJumpPower);
    }

    private void UpdateAll()
    {
        //UpdateLevelUpButton(player.CanLevelUp());
        //UpdateLevel(player.Level);

        UpdateHp(player.GetStatistic(StatisticType.Hp), player.GetStatistic(StatisticType.MaxHp));
        UpdateBaseDamage(0, player.GetStatistic(StatisticType.BaseDamage));
        UpdateAttackSpeed(0, player.GetStatistic(StatisticType.AttackSpeed));
        UpdateCriticalChance(0, player.GetStatistic(StatisticType.CriticalChance));
        UpdateCriticalDamage(0, player.GetStatistic(StatisticType.CriticalDamage));
        UpdateWalkSpeed(0, player.GetStatistic(StatisticType.WalkSpeed));
        UpdateJumpPower(0, player.GetStatistic(StatisticType.JumpPower));
    }

    //private void UpdateLevelUpButton(int exp, int neededExp)
    //{
    //    levelUpButton.SetActive(exp >= neededExp);
    //}

    //private void UpdateLevelUpButton(bool canLevelUp)
    //{
    //    levelUpButton.SetActive(canLevelUp);
    //}

    //private void UpdatePromotionWidget(int promotion)
    //{
    //    promotionButtons.SetActive(promotion > 0);
    //}

    //private void UpdateLevel(int level)
    //{
    //    levelText.text = level.ToString();
    //}

    private void UpdateHp(float hp, float maxHp)
    {
        hpText.text = Mathf.CeilToInt(hp).ToString() + " / " + ((int)maxHp).ToString();
    }

    private void UpdateBaseDamage(float previousValue, float currentValue)
    {
        baseDamageText.text = ((int)currentValue).ToString();
    }

    private void UpdateAttackSpeed(float previousValue, float currentValue)
    {
        attackSpeedText.text = currentValue.ToString("F1");
    }

    private void UpdateCriticalChance(float previousValue, float currentValue)
    {
        criticalChanceText.text = currentValue.ToString("P0");
    }

    private void UpdateCriticalDamage(float previousValue, float currentValue)
    {
        criticalDamageText.text = currentValue.ToString("P0");
    }

    private void UpdateWalkSpeed(float previousValue, float currentValue)
    {
        walkSpeedText.text = ((int)(10 * currentValue)).ToString();
    }

    private void UpdateJumpPower(float previousValue, float currentValue)
    {
        jumpPowerText.text = ((int)currentValue).ToString();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
            Close();
    }

    private void OnDestroy()
    {
        //player.OnExpChange.RemoveListener(UpdateLevelUpButton);
        //player.OnLevelChange.RemoveListener(UpdateLevel);
        //player.OnPromotionChange.RemoveListener(UpdatePromotionWidget);

        closeCallback?.Invoke();

        player.OnHpChange.RemoveListener(UpdateHp);
        player.OnBaseDamageChange.RemoveListener(UpdateBaseDamage);
        player.OnAttackSpeedChange.RemoveListener(UpdateAttackSpeed);
        player.OnCriticalChanceChange.RemoveListener(UpdateCriticalChance);
        player.OnCriticalDamageChange.RemoveListener(UpdateCriticalDamage);
        player.OnWalkSpeedChange.RemoveListener(UpdateWalkSpeed);
        player.OnJumpPowerChange.RemoveListener(UpdateJumpPower);
    }
}
