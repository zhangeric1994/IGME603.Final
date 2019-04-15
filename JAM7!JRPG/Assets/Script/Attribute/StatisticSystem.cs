using System;
using System.Collections.Generic;
using UnityEngine.Events;

public enum AttributeType : int
{
    WalkSpeed_c0 = 0x100,
    JumpPower_c0 = 0x200,
    MaxHp_c0 = 0x300,
    CriticalChance_cp0 = 0x400,
    CriticalDamage_cp0 = 0x500,
    BaseDamage_c0 = 0x600,
    BaseDamage_cp0 = 0x610,
    AttackSpeed_cp0 = 0x700,

    HealingPower_cp0 = 0xF10,
}

public enum StatisticType : int
{
    WalkSpeed = 0x1,
    JumpPower = 0x2,
    MaxHp = 0x3,
    CriticalChance = 0x4,
    CriticalDamage = 0x5,
    BaseDamage = 0x6,
    AttackSpeed = 0x7,

    Hp = 0xF0,
}

public class EventOnStatisticChange : UnityEvent<StatisticType, float, float> { }

public class StatisticSystem
{
    public static float Calculate(StatisticType type, params IAttributeCollection[] attributeSets)
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

    public static Dictionary<StatisticType, float> CalculateAll(params IAttributeCollection[] attributeSets)
    {
        HashSet<int> ids = new HashSet<int>();
        foreach (IAttributeCollection attributes in attributeSets)
            foreach (KeyValuePair<AttributeType, float> attribute in attributes)
                ids.Add((int)attribute.Key >> 8);

        Dictionary<StatisticType, float> statistics = new Dictionary<StatisticType, float>();

        foreach (int id in ids)
        {
            StatisticType statistic = (StatisticType)id;
            statistics.Add(statistic, Calculate(statistic, attributeSets));
        }

        return statistics;
    }

    /// <summary>
    /// An event triggered whenever a certain statistic in this system changes 
    /// </summary>
    public readonly EventOnStatisticChange onStatisticChange = new EventOnStatisticChange();

    //public EventOnDataChange3<StatusEffect> onStatusEffectChange = new EventOnDataChange3<StatusEffect>(); 

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<StatisticType, float> statistics = new Dictionary<StatisticType, float>();

    /// <summary>
    /// Related attribute sets
    /// </summary>
    private IAttributeCollection[] attributeSets;

    ///// <summary>
    ///// All status effects applied to this system in time order
    ///// </summary>
    //private StatusEffectQueue statusEffects = new StatusEffectQueue();

    public float this[StatisticType type]
    {
        get
        {
            return statistics.ContainsKey(type) ? statistics[type] : 0;
        }

        set
        {
            if (!statistics.ContainsKey(type))
            {
                if (value != 0)
                {
                    statistics.Add(type, value);

                    onStatisticChange.Invoke(type, 0, value);
                }
            }
            else
            {
                float previousValue = statistics[type];

                if (value != previousValue)
                {
                    statistics[type] = value;

                    onStatisticChange.Invoke(type, previousValue, value);
                }
            }
        }
    }

    private StatisticSystem()
    {
    }

    public StatisticSystem(params IAttributeCollection[] attributeSets)
    {
        this.attributeSets = attributeSets;

        foreach (IAttributeCollection attributeSet in attributeSets)
            if (attributeSet.OnAttributeChange != null)
                attributeSet.OnAttributeChange.AddListener(UpdateChangedStatistics);

        UpdateChangedStatistics(this.attributeSets);
    }

    ~StatisticSystem()
    {
        foreach (IAttributeCollection attributeSet in attributeSets)
            if (attributeSet != null && attributeSet.OnAttributeChange != null)
                attributeSet.OnAttributeChange.RemoveListener(UpdateChangedStatistics);
    }

    public float Sum(AttributeType type)
    {
        return AttributeSet.Sum(type, attributeSets); //, statusEffects);
    }

    //    public bool AddStatusEffect(StatusEffect statusEffect)
    //    {
    //        bool isExisted = statusEffects.Contains(statusEffect);

    //        if (statusEffects.Push(statusEffect))
    //        {
    //#if UNITY_EDITOR
    //            Debug.Log(LogUtility.MakeLogString("StatisticSystem", "Add " + statusEffect + "\n" + ToString()));
    //#endif

    //            UpdateChangedStatistics(statusEffect);
    //            onStatusEffectChange.Invoke(isExisted ? 0 : 1, statusEffect);

    //            return true;
    //        }

    //        return false;
    //    }

    //    public StatusEffect RemoveStatusEffect(int id)
    //    {
    //        StatusEffect statusEffect = statusEffects.Remove(id);

    //        if (statusEffect != null)
    //        {
    //#if UNITY_EDITOR
    //            Debug.Log(LogUtility.MakeLogString("StatisticSystem", "Remove " + statusEffect + "\n" + ToString()));
    //#endif

    //            UpdateChangedStatistics(statusEffect);
    //            onStatusEffectChange.Invoke(-1, statusEffect);
    //        }

    //        return statusEffect;
    //    }

    public override string ToString()
    {
        string s = "";

        foreach (KeyValuePair<StatisticType, float> statistic in statistics)
            s += ";" + statistic.Key + ":" + statistic.Value;

        return string.Format("Stat: {0}\nTalent: {1}\n\n", s.Substring(1), attributeSets); //, statusEffects);
    }

    public float CalculateModified(StatisticType type, params IAttributeCollection[] modifiers)
    {
        IAttributeCollection[] args = new IAttributeCollection[attributeSets.Length + modifiers.Length];
        Array.Copy(attributeSets, args, attributeSets.Length);
        Array.Copy(modifiers, 0, args, attributeSets.Length, modifiers.Length);

        return Calculate(type, args);
    }

    public Dictionary<StatisticType, float> CalculateAllModified(params IAttributeCollection[] modifiers)
    {
        IAttributeCollection[] args = new IAttributeCollection[attributeSets.Length + modifiers.Length];
        Array.Copy(attributeSets, args, attributeSets.Length);
        Array.Copy(modifiers, 0, args, attributeSets.Length, modifiers.Length);

        return CalculateAll(args);
    }


    private void UpdateChangedStatistics(AttributeType attributeType, float previousValue, float currentValue)
    {
        StatisticType statisticType = (StatisticType)((int)attributeType >> 8);
        this[statisticType] = Calculate(statisticType, attributeSets);
    }

    private void UpdateChangedStatistics(params IAttributeCollection[] attributeSets)
    {
        HashSet<int> changedStatistics = new HashSet<int>();
        foreach (IAttributeCollection attributeSet in attributeSets)
            foreach (KeyValuePair<AttributeType, float> attribute in attributeSet)
                changedStatistics.Add((int)attribute.Key >> 8);

        foreach (int id in changedStatistics)
        {
            StatisticType statisticType = (StatisticType)id;
            this[statisticType] = Calculate(statisticType, attributeSets);
        }
    }

    //private void UpdateChangedStatistics(List<IAttributeCollection> attributeSets)
    //{
    //    HashSet<int> changedStatistics = new HashSet<int>();
    //    foreach (IAttributeCollection attributes in attributeSets)
    //        foreach (KeyValuePair<AttributeType, float> attribute in attributes)
    //            changedStatistics.Add((int)attribute.Key >> 8);

    //    foreach (int id in changedStatistics)
    //    {
    //        StatisticType statistic = (StatisticType)id;
    //        this[statistic] = CalculateStatistic(statistic);
    //    }
    //}

    private void UpdateChangedStatistics(List<StatusEffect> statusEffects)
    {
        HashSet<int> changedStatistics = new HashSet<int>();
        foreach (StatusEffect statusEffect in statusEffects)
            foreach (KeyValuePair<AttributeType, float> attribute in statusEffect)
                changedStatistics.Add((int)attribute.Key >> 8);

        foreach (int id in changedStatistics)
        {
            StatisticType statisticType = (StatisticType)id;
            this[statisticType] = Calculate(statisticType, attributeSets);
        }
    }
}
