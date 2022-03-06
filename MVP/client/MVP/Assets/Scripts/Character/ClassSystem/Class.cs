using UnityEngine;

[CreateAssetMenu(fileName ="ClassSystem/Class", order = 1)]
public class Class : ScriptableObject
{
    public string className;
    public int movementPoint;
    public int lifePoints;
    public int manaPoints;
    public int attackRange;

    public CharacterStatistic strength;
    public CharacterStatistic defense;
    public CharacterStatistic speed;

    public Skill firstSkill;
    public Skill secondSkill;
    public Skill thirdSkill;
}
