using UnityEngine;

[CreateAssetMenu(fileName ="Class", order = 1)]
public class Class : ScriptableObject
{
    public string className;
    public int movementPoint;
    public int lifePoints;
    public int manaPoints;
    public int attackRange;
    public int attackStat;
    public int speedStat;
    public int defenseStat;
}
