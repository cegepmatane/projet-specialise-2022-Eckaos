using UnityEngine;


[CreateAssetMenu(fileName = "Skill", menuName ="Skill/Health Related Skill", order = 2)]
public class HealthRelatedSkill : Skill
{
    public int amount;
    public override void Execute(Character caster, Tile target)
    {
        base.Execute(caster, target);
        this.GetCharacterFromTile(target).classData.lifePoints += amount;
    }
}
