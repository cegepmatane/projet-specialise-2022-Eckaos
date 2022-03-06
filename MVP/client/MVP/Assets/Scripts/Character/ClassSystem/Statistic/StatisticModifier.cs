using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StatisticModifierType
{
    FLAT,
    ADDITIVE_PERCENTAGE,
    MULTIPLIER_PERCENTAGE
}
public class StatisticModifier
{
    public readonly float value;
    public readonly StatisticModifierType type;
    public readonly object source;

    public StatisticModifier(float value, StatisticModifierType type, object source)
    {
        this.value = value;
        this.type = type;
        this.source = source;
    }

    public float ApplyValue(float value)
    {
        if(type != StatisticModifierType.MULTIPLIER_PERCENTAGE) return value += this.value;
        else return value *= 1+this.value;
    }

    public static int CompareModifierOrder(StatisticModifier a, StatisticModifier b)
    {
        if((int)a.type > (int)b.type) return -1;
        else if((int) a.type < (int)b.type) return 1;
        return 0;
    }
}
