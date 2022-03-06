using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Buff Related Skill/Speed Buff Skill", order = 2)]
public class SpeedBuffSkill : BuffRelatedSkill
{
    public override void Execute(Character caster, Tile target)
    {
        base.Execute(caster, target);
        this.GetCharacterFromTile(target).classData.speed.AddModifier(this.CreateModifier());
    }
}
