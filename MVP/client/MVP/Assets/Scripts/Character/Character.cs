using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Character : MonoBehaviour
{
    public Action movementAction;
    public List<SkillAction> skillActions;
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
        skillActions = classData.skills.Select(skill => new SkillAction(this, skill)).ToList<SkillAction>();
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
