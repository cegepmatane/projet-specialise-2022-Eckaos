using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public Action movementAction;
    public List<Action> skillActions;
    public Class classData;

    public int currentActionPoint;
    public int currentMovementPoint;

    public int currentLifePoints;

    private void Awake() {
        var classAssets = Resources.LoadAll<Class>("Class");
        classData = classAssets.GetValue(Random.Range(0, classAssets.Length)) as Class;
        currentLifePoints = classData.lifePoints;
        Reset();
        movementAction = new MovementAction(this);
        skillActions = new List<Action>();
        foreach (Skill skill in classData.skills)
            skillActions.Add(new SkillAction(this, skill));
    }
    private void Start() {
        TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z).player = gameObject;
    }

    public void Reset()
    {
        currentActionPoint = classData.actionPoint;
        currentMovementPoint = classData.movementPoint;
    }
    
    public Action GetAction(int i) => skillActions[i];
    public Tile GetCurrentTile() => TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z);

    public void TakeLifePoints(int amount)
    {
        if(amount >=0) currentLifePoints += amount;
        else currentLifePoints += (int) (amount/classData.defense.value);
        if(currentLifePoints <= 0) DestroyImmediate(gameObject);
    }
}
