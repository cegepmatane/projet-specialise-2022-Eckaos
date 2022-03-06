using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Buff Related Skill/Speed Buff Skill", order = 2)]
public class SpeedBuffSkill : BuffRelatedSkill
{
    public override void Execute(Character caster, Tile target)
    {
        base.Execute(caster, target);
        Character c = GetCharacterFromTile(target);
        if(c == null) return;
        c.classData.speed.AddModifier(this.CreateModifier());
    }
}
