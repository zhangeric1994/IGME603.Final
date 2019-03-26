using UnityEngine;
using UnityEngine.UI;

public class PlayerStatWidget : GUIWidget
{
    [Header("References")]
    [SerializeField] private Text levelText;
    [SerializeField] private Text maxHpText;
    [SerializeField] private Text powerText;
    [SerializeField] private Text dexterityText;
    [SerializeField] private Text wisdomText;
    [SerializeField] private GameObject[] promotions;
    [SerializeField] private Text[] selectableTexts;
    [SerializeField] private GameObject levelUpButton;

    private Player player;
    private string playerID;

    private int choosenIndex;

    private int ChoosenIndex
    {
        set
        {
            selectableTexts[choosenIndex].color = Color.white;

            choosenIndex = value;

            selectableTexts[choosenIndex].color = Color.red;
        }
    }

    public override void Initialize(params object[] args)
    {
        int id = (int)args[0];

        player = Player.GetPlayer(id);
        playerID = id.ToString();
    }

    public override void Show()
    {
        base.Show();

        UpdateAll();

        player.OnExpChange.AddListener(UpdateLevelUpButton);
        player.OnLevelChange.AddListener(UpdateLevel);
        player.OnPromotionChange.AddListener(UpdatePromotionWidget);
        player.OnMaxHpChange.AddListener(UpdateMaxHp);
        player.OnPowerChange.AddListener(UpdatePower);
        player.OnDexterityChange.AddListener(UpdateDexterity);
        player.OnWisdomChange.AddListener(UpdateWisdom);

        ChoosenIndex = 0;

        while (!selectableTexts[choosenIndex].gameObject.activeSelf);
            ChoosenIndex = (choosenIndex + 1 + selectableTexts.Length) % selectableTexts.Length;
    }

    public override void Hide()
    {
        base.Hide();

        player.OnExpChange.RemoveListener(UpdateLevelUpButton);
        player.OnLevelChange.RemoveListener(UpdateLevel);
        player.OnPromotionChange.RemoveListener(UpdatePromotionWidget);
        player.OnMaxHpChange.RemoveListener(UpdateMaxHp);
        player.OnPowerChange.RemoveListener(UpdatePower);
        player.OnDexterityChange.RemoveListener(UpdateDexterity);
        player.OnWisdomChange.RemoveListener(UpdateWisdom);
    }

    private void UpdateAll()
    {
        UpdateLevelUpButton(player.CanLevelUp());
        UpdateLevel(player.Level);
        UpdatePromotionWidget(player.Promotion);
        UpdateMaxHp(player.MaxHp);
        UpdatePower(player.Power);
        UpdateDexterity(player.Dexterity);
        UpdateWisdom(player.Wisdom);
    }

    private void UpdateLevelUpButton(int exp, int neededExp)
    {
        UpdateLevelUpButton(exp >= neededExp);
    }

    private void UpdateLevelUpButton(bool canLevelUp)
    {
        levelUpButton.SetActive(canLevelUp);

        GetComponent<GUIGrid>().Refresh();
    }

    private void UpdatePromotionWidget(int promotion)
    {
        foreach (GameObject go in promotions)
            go.SetActive(promotion > 0);
    }

    private void UpdateLevel(int level)
    {
        levelText.text = level.ToString();
    }

    private void UpdateMaxHp(int maxHp)
    {
        maxHpText.text = maxHp.ToString();
    }

    private void UpdatePower(int power)
    {
        powerText.text = power.ToString();
    }

    private void UpdateDexterity(int dexterity)
    {
        dexterityText.text = dexterity.ToString();
    }

    private void UpdateWisdom(int wisdom)
    {
        wisdomText.text = wisdom.ToString();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Submit" + playerID))
        {
            switch (choosenIndex)
            {
                case 0:
                    player.LevelUp();
                    break;


                case 1:
                    player.IncrementStats(StatsType.Wisdom);
                    break;


                case 2:
                    player.IncrementStats(StatsType.Dexterity);
                    break;


                case 3:
                    player.IncrementStats(StatsType.Power);
                    break;
            }
        }
        else if (Input.GetButtonDown("Up" + playerID))
        {
            do
            {
                ChoosenIndex = (choosenIndex + 1 + selectableTexts.Length) % selectableTexts.Length;
            } while (!selectableTexts[choosenIndex].gameObject.activeInHierarchy);
        }
        else if (Input.GetButtonDown("Down" + playerID))
        {
            do
            {
                ChoosenIndex = (choosenIndex - 1 + selectableTexts.Length) % selectableTexts.Length;
            } while (!selectableTexts[choosenIndex].gameObject.activeInHierarchy);
        }
    }

    private void OnDestroy()
    {
        player.OnExpChange.RemoveListener(UpdateLevelUpButton);
        player.OnLevelChange.RemoveListener(UpdateLevel);
        player.OnPromotionChange.RemoveListener(UpdatePromotionWidget);
        player.OnMaxHpChange.RemoveListener(UpdateMaxHp);
    }
}
