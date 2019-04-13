using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AttributeType : int
{
}

public enum Statistic : int
{
    Hp = 1,
    Strength = 2,
    Agility = 3,
    Intelligence = 4,
}

public class StatisticSystem
{
    public class EventOnStatisticChange : UnityEvent<Statistic, float, float> { }

    /// <summary>
    /// An event triggered whenever a certain statistic in this system changes 
    /// </summary>
    public EventOnStatisticChange onStatisticChange = new EventOnStatisticChange();

    public EventOnDataChange3<StatusEffect> onStatusEffectChange = new EventOnDataChange3<StatusEffect>(); 

    /// <summary>
    /// 
    /// </summary>
    private Dictionary<Statistic, float> statistics = new Dictionary<Statistic, float>();

    /// <summary>
    /// Attributes that will never change over time
    /// </summary>
    private readonly AttributeSet talents;

    /// <summary>
    /// All status effects applied to this system in time order
    /// </summary>
    private StatusEffectQueue statusEffects = new StatusEffectQueue();

    public float this[Statistic statistic]
    {
        get
        {
            return statistics.ContainsKey(statistic) ? CalculateStatistic(statistic) : 0;
        }

        set
        {
            bool hasChange = true;
            float originalValue = 0;

            if (!statistics.ContainsKey(statistic))
                statistics.Add(statistic, value);
            else
            {
                originalValue = statistics[statistic];

                if (value != originalValue)
                    statistics[statistic] = value;
                else
                    hasChange = false;
            }

            if (hasChange)
                onStatisticChange.Invoke(statistic, originalValue, statistics[statistic]);
        }
    }

    private StatisticSystem()
    {
        talents = new AttributeSet();
    }

    public StatisticSystem(AttributeSet talents)
    {
        this.talents = talents;

        UpdateChangedStatistics(this.talents);
    }

    public float Sum(AttributeType attribute)
    {
        return AttributeSet.Sum(attribute, talents, statusEffects);
    }

    public float CalculateStatistic(Statistic statistic)
    {
        return CalculateStatistic(statistic, talents, statusEffects);
    }

    public static float CalculateStatistic(Statistic statistic, params IAttributeCollection[] attributeSets)
    {
        switch (statistic)
        {
            default:
                return 0;
        }
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

    //internal void HandleRoundNumberChange(int round)
    //{
    //    List<StatusEffect> pastStatusEffects = new List<StatusEffect>();

    //    while (statusEffects.Top() != null && statusEffects.Top().EndRound == round)
    //        pastStatusEffects.Add(statusEffects.Pop());

    //    UpdateChangedStatistics(pastStatusEffects);

    //    foreach (StatusEffect statusEffect in pastStatusEffects)
    //        onStatusEffectChange.Invoke(-1, statusEffect);
    //}

    internal void UpdateChangedStatistics(IAttributeCollection attributes)
    {
        HashSet<int> changedStatistics = new HashSet<int>();
        foreach (KeyValuePair<int, float> attribute in attributes)
            changedStatistics.Add(attribute.Key / 10);

        foreach (int id in changedStatistics)
        {
            Statistic statistic = (Statistic)id;
            this[statistic] = CalculateStatistic(statistic);
        }
    }

    internal void UpdateChangedStatistics(List<StatusEffect> statusEffects)
    {
        HashSet<int> changedStatistics = new HashSet<int>();
        foreach (StatusEffect statusEffect in statusEffects)
            foreach (KeyValuePair<int, float> attribute in statusEffect)
                changedStatistics.Add(attribute.Key / 10);

        foreach (int id in changedStatistics)
        {
            Statistic statistic = (Statistic)id;
            this[statistic] = CalculateStatistic(statistic);
        }
    }

    public override string ToString()
    {
        string s = "";

        foreach (KeyValuePair<Statistic, float> statistic in statistics)
            s += ";" + statistic.Key + ":" + statistic.Value;

        return string.Format("Stat: {0}\nTalent: {1}\n\n{2}", s.Substring(1), talents, statusEffects);
    }
}
