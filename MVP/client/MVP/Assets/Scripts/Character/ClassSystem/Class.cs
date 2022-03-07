using UnityEngine;

[CreateAssetMenu(fileName ="ClassSystem/Class", order = 1)]
public class Class : ScriptableObject
{
    public string className;
    public int movementPoint;
    public int lifePoints;
    public int manaPoints;
    public int attackRange;
    
    [Range(1, 100)]
    public int speed;

    public CharacterStatistic strength;
    public CharacterStatistic defense;

    public Skill firstSkill;
    public Skill secondSkill;
    public Skill thirdSkill;
}
