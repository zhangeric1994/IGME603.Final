using System.Collections.Generic;

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
    private StatisticSystem stats;

    public EventOnDataChange2<float> OnWalkSpeedChange { get; private set; }
    public EventOnDataChange2<float> OnJumpPowerChange { get; private set; }
    public EventOnDataChange2<float> OnMaxHpChange { get; private set; }
    public EventOnDataChange2<float> OnCriticalChanceChange { get; private set; }
    public EventOnDataChange2<float> OnCriticalDamageChange { get; private set; }
    public EventOnDataChange2<float> OnBaseDamageChange { get; private set; }
    public EventOnDataChange2<float> OnAttackSpeedChange { get; private set; }


    public float MaxHp
    {
        get
        {
            return stats[Statistic.MaxHp];
        }
    }


    private Player(int id, int maxHp, int power, int dexterity, int wisdom)
    {
        this.id = id;


        AttributeSet initialAttributes = new AttributeSet();
        initialAttributes.Add(AttributeType.WalkSpeed_c0, 0.8f);
        initialAttributes.Add(AttributeType.JumpPower_c0, 120);
        initialAttributes.Add(AttributeType.MaxHp_c0, 5);
        initialAttributes.Add(AttributeType.CriticalChance_cp0, 0.05f);
        initialAttributes.Add(AttributeType.BaseDamage_cp0, 1);

        stats = new StatisticSystem(initialAttributes);
        UnityEngine.Debug.Log(stats);
        stats.OnStatisticChange.AddListener(DispatchStatisticChangeEvents);


        OnWalkSpeedChange = new EventOnDataChange2<float>();
        OnJumpPowerChange = new EventOnDataChange2<float>();
        OnMaxHpChange = new EventOnDataChange2<float>();
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

    public float GetStatistic(Statistic statistic)
    {
        return stats[statistic];
    }


    private void DispatchStatisticChangeEvents(Statistic statistic, float previousValue, float currentValue)
    {
        switch (statistic)
        {
            case Statistic.WalkSpeed:
                OnWalkSpeedChange.Invoke(previousValue, currentValue);
                break;


            case Statistic.JumpPower:
                OnJumpPowerChange.Invoke(previousValue, currentValue);
                break;


            case Statistic.MaxHp:
                OnMaxHpChange.Invoke(previousValue, currentValue);
                break;


            case Statistic.CriticalChance:
                OnCriticalChanceChange.Invoke(previousValue, currentValue);
                break;


            case Statistic.CriticalDamage:
                OnCriticalDamageChange.Invoke(previousValue, currentValue);
                break;


            case Statistic.BaseDamage:
                OnBaseDamageChange.Invoke(previousValue, currentValue);
                break;


            case Statistic.AttackSpeed:
                OnAttackSpeedChange.Invoke(previousValue, currentValue);
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
