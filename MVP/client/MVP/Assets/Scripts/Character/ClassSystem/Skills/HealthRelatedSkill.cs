using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Health Related Skill", order = 2)]
public class HealthRelatedSkill : Skill
{
    public int amount;
    public override void Execute(Character caster, Tile target)
    {
        base.Execute(caster, target);
        Character c = GetCharacterFromTile(target);
        if(c == null) return;
        c.classData.lifePoints += amount;
    }
}
