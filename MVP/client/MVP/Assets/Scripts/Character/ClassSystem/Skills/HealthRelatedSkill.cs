using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Health Related Skill", order = 2)]
public class HealthRelatedSkill : Skill
{
    public int amount;
    public override void Execute(Character caster, Tile target)
    {
        if(target == null) return;
        Character c = GetCharacterFromTile(target);
        if(c == null) return;
        c.TakeLifePoints(amount);
    }
}
