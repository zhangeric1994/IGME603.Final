using System.Collections.Generic;

public class Player
{
    private static Dictionary<int, Player> playerList = new Dictionary<int, Player>();

    public static Player GetPlayer(int id)
    {
        return playerList[id];
    }

    public static void CreatePlayer(int id, int maxHp, int power, int dexterity, int wisdom)
    {
        if (!playerList.ContainsKey(id))
            playerList[id] = new Player(id, maxHp, power, dexterity, wisdom);
    }

    public readonly int id;
    public readonly HeroType playerClass;

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
    public EventOnDataChange1<int> OnMaxHpChange { get; private set; }
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

    public int MaxHp
    {
        get
        {
            return maxHp;
        }

        private set
        {
            if (value != maxHp)
            {
                maxHp = value;

                OnMaxHpChange.Invoke(maxHp);
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

    private Player(int id, int maxHp, int power, int dexterity, int wisdom)
    {
        this.id = id;
        playerClass = 0;

        level = 1;
        exp = 0;
        neededExp = 100;
        promotion = 3;

        this.maxHp = maxHp;
        this.power = power;
        this.dexterity = dexterity;
        this.wisdom = wisdom;

        OnLevelChange = new EventOnDataChange1<int>();
        OnExpChange = new EventOnDataChange2<int>();
        OnNeededExpChange = new EventOnDataChange1<int>();
        OnPromotionChange = new EventOnDataChange1<int>();
        OnMaxHpChange = new EventOnDataChange1<int>();
        OnPowerChange = new EventOnDataChange1<int>();
        OnDexterityChange = new EventOnDataChange1<int>();
        OnWisdomChange = new EventOnDataChange1<int>();
    }

    public void IncrementStats(StatsType type)
    {
        if (Promotion > 0)
        {
            // overwrite current Stats in that type
            switch (type)
            {
                case StatsType.Power:
                    Power++;
                    break;


                case StatsType.Dexterity:
                    Dexterity++;
                    break;


                case StatsType.Wisdom:
                    Wisdom++;
                    break;
            }

            Promotion--;
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

            switch (playerClass)
            {
                case HeroType.Knight:
                    dexterity++;
                    break;
                case HeroType.Nurse:
                    wisdom++;
                    break;
                case HeroType.Fat:
                    maxHp++;
                    break;
            }

            return CanLevelUp();
        }

        return false;
    }
}
