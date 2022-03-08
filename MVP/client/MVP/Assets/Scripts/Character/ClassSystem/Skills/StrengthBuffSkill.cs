using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Buff Related Skill/Strength Buff Skill", order = 2)]
public class StrengthBuffSkill : BuffRelatedSkill
{
    public override void Execute(Character caster, Tile target)
    {
        Character c = GetCharacterFromTile(target);
        if(c == null) return;
        c.classData.strength.AddModifier(this.CreateModifier());
    }
}
