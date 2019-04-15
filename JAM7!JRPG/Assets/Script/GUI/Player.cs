using System.Collections.Generic;
using UnityEngine;

public enum PlayerClass
{
    Knight,
    Heavy,
    Nurse,
}

public class Player
{
    private static Dictionary<int, Player> playerList = new Dictionary<int, Player>();

    public static Player GetPlayer(int id)
    {
#if UNITY_EDITOR
        if (!playerList.ContainsKey(id))
            playerList.Add(id, new Player(id, 0, 0, 0, 0));
#endif

        return playerList[id];
    }

    public static void CreatePlayer(int id, int maxHp, int power, int dexterity, int wisdom)
    {
        if (!playerList.ContainsKey(id))
            playerList.Add(id, new Player(id, maxHp, power, dexterity, wisdom));
    }


    public readonly int id;

    private AttributeSet talents = new AttributeSet();
    public readonly Inventory inventory;
    public readonly StatisticSystem stats;

    public EventOnDataChange2<float> OnWalkSpeedChange { get; private set; }
    public EventOnDataChange2<float> OnJumpPowerChange { get; private set; }
    public EventOnDataChange2<float> OnHpChange { get; private set; }
    public EventOnDataChange2<float> OnCriticalChanceChange { get; private set; }
    public EventOnDataChange2<float> OnCriticalDamageChange { get; private set; }
    public EventOnDataChange2<float> OnBaseDamageChange { get; private set; }
    public EventOnDataChange2<float> OnAttackSpeedChange { get; private set; }


    private Player(int id, int maxHp, int power, int dexterity, int wisdom)
    {
        this.id = id;

        talents.Add(AttributeType.WalkSpeed_c0, 0.8f);
        talents.Add(AttributeType.JumpPower_c0, 150);
        talents.Add(AttributeType.MaxHp_c0, 10);
        talents.Add(AttributeType.CriticalChance_cp0, 0.05f);
        talents.Add(AttributeType.CriticalDamage_cp0, 1.5f);
        talents.Add(AttributeType.BaseDamage_cp0, 1);
        talents.Add(AttributeType.AttackSpeed_cp0, 1);

        inventory = new Inventory();
        inventory.Add(1, 1);
        inventory.Add(2, 1);
        inventory.Add(3, 1);

        stats = new StatisticSystem(talents, inventory);
        stats[StatisticType.Hp] = stats[StatisticType.MaxHp];
        stats.onStatisticChange.AddListener(DispatchStatisticChangeEvents);


        OnWalkSpeedChange = new EventOnDataChange2<float>();
        OnJumpPowerChange = new EventOnDataChange2<float>();
        OnHpChange = new EventOnDataChange2<float>();
        OnCriticalChanceChange = new EventOnDataChange2<float>();
        OnCriticalDamageChange = new EventOnDataChange2<float>();
        OnBaseDamageChange = new EventOnDataChange2<float>();
        OnAttackSpeedChange = new EventOnDataChange2<float>();


        playerClass = 0;

        level = 1;
        exp = 0;
        neededExp = 100;

        this.maxHp = maxHp;
        this.power = power;
        this.dexterity = dexterity;
        this.wisdom = wisdom;

        OnLevelChange = new EventOnDataChange1<int>();
        OnExpChange = new EventOnDataChange2<int>();
        OnNeededExpChange = new EventOnDataChange1<int>();
        OnPromotionChange = new EventOnDataChange1<int>();
        OnPowerChange = new EventOnDataChange1<int>();
        OnDexterityChange = new EventOnDataChange1<int>();
        OnWisdomChange = new EventOnDataChange1<int>();
    }

    public float GetStatistic(StatisticType statistic)
    {
        return stats[statistic];
    }

    public void Loot(Loot loot)
    {
        inventory.Add(loot.Id);
    }

    public float ApplyDamage(float rawDamage)
    {
        float damagea = rawDamage;

        stats[StatisticType.Hp] = Mathf.Max(0, stats[StatisticType.Hp] - damagea);

        return damagea;
    }

    public float ApplyHealing(float rawHealing)
    {
        float healing = rawHealing;

        stats[StatisticType.Hp] = Mathf.Min(stats[StatisticType.MaxHp], stats[StatisticType.Hp] + healing);

        return healing;
    }

    public float ApplyHealing(params IAttributeCollection[] attributeSets)
    {
        float sum = 0;

        foreach (IAttributeCollection attributeSet in attributeSets)
            sum += attributeSet[AttributeType.HealingPower_cp0];

        return ApplyHealing(sum);
    }

    private void DispatchStatisticChangeEvents(StatisticType statistic, float previousValue, float currentValue)
    {
#if UNITY_EDITOR
        Debug.Log(LogUtility.MakeLogStringFormat("Player", "{0}: {1} ==> {2}", statistic, previousValue, currentValue));
#endif

        switch (statistic)
        {
            case StatisticType.WalkSpeed:
                OnWalkSpeedChange.Invoke(previousValue, currentValue);
                break;


            case StatisticType.JumpPower:
                OnJumpPowerChange.Invoke(previousValue, currentValue);
                break;


            case StatisticType.MaxHp:
                stats[StatisticType.Hp] += currentValue - previousValue;
                break;


            case StatisticType.CriticalChance:
                OnCriticalChanceChange.Invoke(previousValue, currentValue);
                break;


            case StatisticType.CriticalDamage:
                OnCriticalDamageChange.Invoke(previousValue, currentValue);
                break;


            case StatisticType.BaseDamage:
                OnBaseDamageChange.Invoke(previousValue, currentValue);
                break;


            case StatisticType.AttackSpeed:
                OnAttackSpeedChange.Invoke(previousValue, currentValue);
                break;


            case StatisticType.Hp:
                OnHpChange.Invoke(currentValue, stats[StatisticType.MaxHp]);
                break;
        }
    }


    public readonly PlayerClass playerClass;

    private int level;
    private int exp;
    private int neededExp;
    private int promotion;

    private int maxHp;
    private int power;
    private int dexterity;
    private int wisdom;

    public EventOnDataChange1<int> OnLevelChange { get; private set; }
    public EventOnDataChange2<int> OnExpChange { get; private set; }
    public EventOnDataChange1<int> OnNeededExpChange { get; private set; }
    public EventOnDataChange1<int> OnPromotionChange { get; private set; }
    public EventOnDataChange1<int> OnPowerChange { get; private set; }
    public EventOnDataChange1<int> OnDexterityChange { get; private set; }
    public EventOnDataChange1<int> OnWisdomChange { get; private set; }

    public int Level
    {
        get
        {
            return level;
        }

        private set
        {
            if (value != level)
            {
                level = value;

                OnLevelChange.Invoke(level);
            }
        }
    }

    public int Exp
    {
        get
        {
            return exp;
        }

        private set
        {
            if (value != exp)
            {
                exp = value;

                OnExpChange.Invoke(exp, neededExp);
            }
        }
    }

    public int NeededExp
    {
        get
        {
            return neededExp;
        }

        private set
        {
            if (value != neededExp)
            {
                neededExp = value;

                OnExpChange.Invoke(exp, neededExp);
                OnNeededExpChange.Invoke(neededExp);
            }
        }
    }

    public int Promotion
    {
        get
        {
            return promotion;
        }

        private set
        {
            if (value != promotion)
            {
                promotion = value;

                OnPromotionChange.Invoke(promotion);
            }
        }
    }

    public int Power
    {
        get
        {
            return power;
        }

        private set
        {
            if (value != power)
            {
                power = value;

                OnPowerChange.Invoke(power);
            }
        }
    }

    public int Dexterity
    {
        get
        {
            return dexterity;
        }

        private set
        {
            if (value != dexterity)
            {
                dexterity = value;

                OnDexterityChange.Invoke(dexterity);
            }
        }
    }

    public int Wisdom
    {
        get
        {
            return wisdom;
        }

        private set
        {
            if (value != wisdom)
            {
                wisdom = value;

                OnWisdomChange.Invoke(wisdom);
            }
        }
    }

    public void SetStats(int value, StatsType type, bool overwrite = false)
    {
        // overwrite current Stats in that type
        switch (type)
        {
            case StatsType.Power:
                Power = overwrite ? power + value : value;
                break;

            case StatsType.Dexterity:
                Dexterity = overwrite ? dexterity + value : value;
                break;

            case StatsType.Wisdom:
                Wisdom = overwrite ? wisdom + value : value;
                break;
        }
    }

    public bool CanLevelUp()
    {
        return exp >= NeededExp;
    }

    public bool LevelUp()
    {
        if (CanLevelUp())
        {
            Level++;
            Promotion++;
            NeededExp *= 2;

            return CanLevelUp();
        }

        return false;
    }
}
