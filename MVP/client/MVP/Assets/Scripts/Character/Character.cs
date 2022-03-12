using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    public Class classData;
    public int currentActionPoint;
    public int currentMovementPoint;
    public int currentLifePoints;

    public string playerId;
    public void SetClass(Class c)
    {
        classData = c;
        currentLifePoints = classData.lifePoints;
        Reset();
    } 

    public void SetPlayer(string id) => playerId = id;
    public void Reset()
    {
        currentActionPoint = classData.actionPoint;
        currentMovementPoint = classData.movementPoint;
    }

    public void TakeLifePoints(int amount)
    {
        if(amount >=0) currentLifePoints += amount;
        else currentLifePoints += (int) (amount/classData.defense.value);
        if(currentLifePoints <= 0) DestroyImmediate(gameObject);
    }
    public Skill GetSkill(int index) => classData.skills[index];
}
