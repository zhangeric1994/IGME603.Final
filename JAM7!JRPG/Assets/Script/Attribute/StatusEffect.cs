using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffectData
{
    public int MaxNumStacks;
    public string Name;
    public string Attributes;
}

public class StatusEffect : IAttributeCollection, IComparable
{
    private readonly AttributeSet attributes;

    public StatusEffectData Data { get; internal set; }

    public int Id { get; internal set; }

    public int NumStacks { get; internal set; }
    public float EndTime { get; internal set; }
    
    public int MaxNumStacks
    {
        get
        {
            return Data.MaxNumStacks;
        }
    }

    public EventOnAttributeChange OnAttributeChange => new EventOnAttributeChange();

    //public float this[int id]
    //{
    //    get
    //    {
    //        return attributes[id] * NumStacks;
    //    }
    //}

    public float this[AttributeType type]
    {
        get
        {
            return attributes[type] * NumStacks;
        }
    }

    private StatusEffect()
    {
    }

    public StatusEffect(int id, float duration, int numStacks = 1)
    {
        Id = id;
        EndTime = Time.time + duration;
        NumStacks = numStacks;

        attributes = AttributeSet.Parse(Data.Attributes);
    }

    public bool ReachMaxNumStacks()
    {
        return NumStacks == MaxNumStacks;
    }

    internal void Stack(StatusEffect other)
    {
        if (NumStacks != MaxNumStacks)
            NumStacks = Math.Min(NumStacks + other.NumStacks, MaxNumStacks);

        if (other.EndTime > EndTime)
            EndTime = other.EndTime;
    }

    public int CompareTo(StatusEffect other)
    {
        return EndTime.CompareTo(other.EndTime);
    }

    public int CompareTo(object obj)
    {
        return CompareTo((StatusEffect)obj);
    }

    public IEnumerator<KeyValuePair<AttributeType, float>> GetEnumerator()
    {
        return new Enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return new Enumerator(this);
    }

    public override string ToString()
    {
        return string.Format("[{0}][x{1}/{2}] Id:{3} Name:{4} Attributes:{5}", EndTime, NumStacks, MaxNumStacks, Id, Data.Name, attributes);
    }

    public class Enumerator : IEnumerator<KeyValuePair<AttributeType, float>>
    {
        private int numStacks;
        private IEnumerator<KeyValuePair<AttributeType, float>> attributeSetEnumerator;

        public Enumerator(StatusEffect statusEffect)
        {
            numStacks = statusEffect.NumStacks;
            attributeSetEnumerator = statusEffect.attributes.GetEnumerator();
        }

        public KeyValuePair<AttributeType, float> Current
        {
            get
            {
                return new KeyValuePair<AttributeType, float>(attributeSetEnumerator.Current.Key, attributeSetEnumerator.Current.Value * numStacks);
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Dispose() {}

        public bool MoveNext()
        {
            return attributeSetEnumerator.MoveNext();
        }

        public void Reset()
        {
            attributeSetEnumerator.Reset();
        }
    }
}
