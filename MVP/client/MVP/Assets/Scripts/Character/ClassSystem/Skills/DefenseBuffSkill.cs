using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Buff Related Skill/Defense Buff Skill", order = 2)]
public class DefenseBuffSkill : BuffRelatedSkill
{
    public override void Execute(Character caster, Tile target)
    {
        base.Execute(caster, target);
        this.GetCharacterFromTile(target).classData.defense.AddModifier(this.CreateModifier());
    }
}
