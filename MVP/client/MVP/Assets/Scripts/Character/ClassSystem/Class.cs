using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName ="ClassSystem/Class", order = 1)]
public class Class : ScriptableObject
{
    public string className;

    [Range(1,500)]
    public int lifePoints = 1;
    [Range(1,100)]
    public int actionPoint;
    [Range(1,10)]
    public int movementPoint = 1;

    [Range(1,7)]
    public int attackRange = 1;

    [Range(1, 100)]
    public int speed;

    public CharacterStatistic strength;
    public CharacterStatistic defense;

    public Skill[] skills = new Skill[4];
}
