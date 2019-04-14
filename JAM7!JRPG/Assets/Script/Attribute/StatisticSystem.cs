using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttributeType : int
{
    WalkSpeed_c0 = 0x100,
    JumpPower_c0 = 0x200,
    MaxHp_c0 = 0x300,
    CriticalChance_cp0 = 0x400,
    CriticalDamage_cp0 = 0x500,
    BaseDamage_cp0 = 0x600,
    AttackSpeed_cp0 = 0x700,
}

public enum StatisticType : int
{
    WalkSpeed = 1,
    JumpPower = 2,
    MaxHp = 3,
    CriticalChance = 4,
    CriticalDamage = 5,
    BaseDamage = 6,
    AttackSpeed = 7,
}

public class StatisticSystem
{
    public static float CalculateStatistic(StatisticType type, params IAttributeCollection[] attributeSets)
    {
        switch (type)
        {
            case StatisticType.WalkSpeed:
                return AttributeSet.Sum(AttributeType.WalkSpeed_c0, attributeSets);


            case StatisticType.JumpPower:
                return AttributeSet.Sum(AttributeType.JumpPower_c0, attributeSets);


            case StatisticType.MaxHp:
                return AttributeSet.Sum(AttributeType.MaxHp_c0, attributeSets);


            case StatisticType.CriticalChance:
                return AttributeSet.Sum(AttributeType.CriticalChance_cp0, attributeSets);


            case StatisticType.CriticalDamage:
                return AttributeSet.Sum(AttributeType.CriticalDamage_cp0, attributeSets);


            case StatisticType.BaseDamage:
                return AttributeSet.Sum(AttributeType.BaseDamage_cp0, attributeSets);


            case StatisticType.AttackSpeed:
                return AttributeSet.Sum(AttributeType.AttackSpeed_cp0, attributeSets);


            default:
                return 0;
        }
    }


    public class EventOnStatisticChange : UnityEvent<StatisticType, float, float> { }

    /// <summary>
    /// An event triggered whenever a certain statistic in this system changes 
    /// </summary>
    public EventOnStatisticChange OnStatisticChange = new EventOnStatisticChange();

    public EventOnDataChange3<StatusEffect> onStatusEffectChange = new EventOnDataChange3<StatusEffect>(); 

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<StatisticType, float> statistics = new Dictionary<StatisticType, float>();

    /// <summary>
    /// Default values of attributes
    /// </summary>
    private readonly AttributeSet initialValues;

    /// <summary>
    /// All status effects applied to this system in time order
    /// </summary>
    private StatusEffectQueue statusEffects = new StatusEffectQueue();

    public float this[StatisticType type]
    {
        get
        {
            return statistics.ContainsKey(type) ? CalculateStatistic(type) : 0;
        }

        set
        {
            bool hasChange = true;
            float originalValue = 0;

            if (!statistics.ContainsKey(type))
                statistics.Add(type, value);
            else
            {
                originalValue = statistics[type];

                if (value != originalValue)
                    statistics[type] = value;
                else
                    hasChange = false;
            }

            if (hasChange)
                OnStatisticChange.Invoke(type, originalValue, statistics[type]);
        }
    }

    private StatisticSystem()
    {
        initialValues = new AttributeSet();
    }

    public StatisticSystem(AttributeSet initialValues)
    {
        this.initialValues = initialValues;

        UpdateChangedStatistics(this.initialValues);
    }

    public float Sum(AttributeType type)
    {
        return AttributeSet.Sum(type, initialValues, statusEffects);
    }

    public float CalculateStatistic(StatisticType type)
    {
        return CalculateStatistic(type, initialValues, statusEffects);
    }

    public bool AddStatusEffect(StatusEffect statusEffect)
    {
        bool isExisted = statusEffects.Contains(statusEffect);

        if (statusEffects.Push(statusEffect))
        {
#if UNITY_EDITOR
            Debug.Log(LogUtility.MakeLogString("StatisticSystem", "Add " + statusEffect + "\n" + ToString()));
#endif

            UpdateChangedStatistics(statusEffect);
            onStatusEffectChange.Invoke(isExisted ? 0 : 1, statusEffect);

            return true;
        }

        return false;
    }

    public StatusEffect RemoveStatusEffect(int id)
    {
        StatusEffect statusEffect = statusEffects.Remove(id);

        if (statusEffect != null)
        {
#if UNITY_EDITOR
            Debug.Log(LogUtility.MakeLogString("StatisticSystem", "Remove " + statusEffect + "\n" + ToString()));
#endif

            UpdateChangedStatistics(statusEffect);
            onStatusEffectChange.Invoke(-1, statusEffect);
        }

        return statusEffect;
    }

    public override string ToString()
    {
        string s = "";

        foreach (KeyValuePair<StatisticType, float> statistic in statistics)
            s += ";" + statistic.Key + ":" + statistic.Value;

        return string.Format("Stat: {0}\nTalent: {1}\n\n{2}", s.Substring(1), initialValues, statusEffects);
    }


    private void UpdateChangedStatistics(IAttributeCollection attributes)
    {
        HashSet<int> changedStatistics = new HashSet<int>();
        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
            changedStatistics.Add((int)attribute.Key >> 8);

        foreach (int id in changedStatistics)
        {
            StatisticType statistic = (StatisticType)id;
            this[statistic] = CalculateStatistic(statistic);
        }
    }

    private void UpdateChangedStatistics(List<StatusEffect> statusEffects)
    {
        HashSet<int> changedStatistics = new HashSet<int>();
        foreach (StatusEffect statusEffect in statusEffects)
            foreach (KeyValuePair<AttributeType, float> attribute in statusEffect)
                changedStatistics.Add((int)attribute.Key >> 8);

        foreach (int id in changedStatistics)
        {
            StatisticType statistic = (StatisticType)id;
            this[statistic] = CalculateStatistic(statistic);
        }
    }
}
