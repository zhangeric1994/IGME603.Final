using System.Collections.Generic;

public enum PlayerClass
{
    Knight,
}

public class Player
{
    private static Dictionary<int, Player> playerList = new Dictionary<int, Player>();

    public static Player GetPlayer(int id)
    {
        return playerList[id];
    }

    public static void CreatePlayer(int id)
    {
        if (!playerList.ContainsKey(id))
            playerList[id] = new Player(id);
    }

    public readonly int id;
    public readonly PlayerClass playerClass;

    private int level;
    private int exp;
    private int neededExp;
    private int promotion;
    private int maxHp;
    
    public EventOnDataChange1<int> OnLevelChange { get; private set; }
    public EventOnDataChange2<int> OnExpChange { get; private set; }
    public EventOnDataChange1<int> OnNeededExpChange { get; private set; }
    public EventOnDataChange1<int> OnPromotionChange { get; private set; }
    public EventOnDataChange1<int> OnMaxHpChange { get; private set; }

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

    private Player(int id)
    {
        this.id = id;
        playerClass = 0;

        level = 1;
        exp = 0;
        neededExp = 100;
        maxHp = 5;

        OnLevelChange = new EventOnDataChange1<int>();
        OnExpChange = new EventOnDataChange2<int>();
        OnNeededExpChange = new EventOnDataChange1<int>();
        OnPromotionChange = new EventOnDataChange1<int>();
        OnMaxHpChange = new EventOnDataChange1<int>();
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
