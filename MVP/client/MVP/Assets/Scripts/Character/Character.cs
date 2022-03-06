using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private CharacterAction attackAction;
    private CharacterAction movementAction;
    private CharacterAction skillAction;
    public Class classData;

    private void Start() {
        var classAssets = Resources.FindObjectsOfTypeAll<Class>();
        TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z).player = gameObject;
        classData = classAssets.GetValue(Random.Range(0, classAssets.Length)) as Class;
        attackAction = new CharacterAttack(this, classData.attackRange);
        movementAction = new CharacterMovement(this, classData.movementPoint);
        skillAction = new CharacterSkill(this, classData.firstSkill);
    }

    void Update()
    {
        if(movementAction.IsSelecting())
            movementAction.TileSelection();
        else if(attackAction.IsSelecting())
            attackAction.TileSelection();
        else if(skillAction.IsSelecting())
            skillAction.TileSelection();
        
        if(movementAction.IsExecuting())
            movementAction.Execute();
        else if(attackAction.IsExecuting())
            attackAction.Execute();
        else if(skillAction.IsExecuting())
            skillAction.Execute();
    }


    public void InMovingRangeSelectableTile() 
    {
        if(!attackAction.IsUsed() && !skillAction.IsUsed())
        movementAction.GetSelectableTiles();
    }
    public void InAttackRangeSelectableTile() 
    {
        if(!movementAction.IsUsed() && !skillAction.IsUsed())
        attackAction.GetSelectableTiles();
    }

    public void InSkillRangeSelectableTile()
    {
        if(!movementAction.IsUsed() && !attackAction.IsUsed())
        skillAction.GetSelectableTiles();
    }
}
