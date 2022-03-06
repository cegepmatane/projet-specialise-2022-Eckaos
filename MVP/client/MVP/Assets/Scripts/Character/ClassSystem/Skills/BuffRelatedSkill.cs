using UnityEngine;
public abstract class BuffRelatedSkill : Skill
{
    public float percentage;
    protected StatisticModifier CreateModifier() => new StatisticModifier(percentage, StatisticModifierType.ADDITIVE_PERCENTAGE, this);
}
