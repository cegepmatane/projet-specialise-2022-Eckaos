using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Collections.ObjectModel;

[Serializable]
public class CharacterStatistic
{
    public int baseValue;
    public float value { get { return CalculateFinalValue(); } }
    private readonly List<StatisticModifier> statisticModifiers;
    private bool hasChanged = true;

    private CharacterStatistic()
    {
        
        statisticModifiers = new List<StatisticModifier>();
    }
    public CharacterStatistic(int baseValue) : this()
    {
        this.baseValue = baseValue;
    }

    public ReadOnlyCollection<StatisticModifier> GetModifiers() => statisticModifiers.AsReadOnly();
    public void AddModifier(StatisticModifier modifier) 
    {
        statisticModifiers.Add(modifier);
        statisticModifiers.Sort(StatisticModifier.CompareModifierOrder);
    } 
    public bool RemoveModifier(StatisticModifier modifier) => statisticModifiers.Remove(modifier);
    public bool RemoveAllModifierFromSource(object source)
    {
        bool didRemove = false;
        for (int i = statisticModifiers.Count - 1; i >= 0 ; i--)
        {
            if(statisticModifiers[i].source == source)
            {
                statisticModifiers.RemoveAt(i);
                didRemove = true;
            }
        }
        return didRemove;
    }
 
    private float CalculateFinalValue()
    {
        float finalValue = baseValue;
        float sumPercentageAdd = 0;
        for (int i = 0; i < statisticModifiers.Count; i++)
        {
            if(StatisticModifierType.FLAT == statisticModifiers[i].type)
                finalValue += statisticModifiers[i].value;
            if(StatisticModifierType.ADDITIVE_PERCENTAGE == statisticModifiers[i].type)
            {
                sumPercentageAdd += statisticModifiers[i].value;
                if(i+1 >= statisticModifiers.Count || statisticModifiers[i+1].type != StatisticModifierType.ADDITIVE_PERCENTAGE)
                {
                    finalValue *= 1+sumPercentageAdd;
                    sumPercentageAdd = 0;
                }
            }
            if(StatisticModifierType.MULTIPLIER_PERCENTAGE == statisticModifiers[i].type)
                finalValue *= 1+statisticModifiers[i].value;
        }
            
        return (float)Mathf.Round(finalValue);
    }
}
