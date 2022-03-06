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
        TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z).player = gameObject;
        var classAssets = Resources.LoadAll<Class>("Class");
        classData = classAssets.GetValue(Random.Range(0, classAssets.Length)) as Class;
        attackAction = new CharacterAttack(this, classData.attackRange);
        movementAction = new CharacterMovement(this, classData.movementPoint);
        skillAction = new CharacterSkill(this);
    }

    void Update()
    {
        if(movementAction == null || skillAction == null || attackAction == null) return;
        if(movementAction.IsSelecting())
            movementAction.TileSelection();
        else if(attackAction.IsSelecting())
            attackAction.TileSelection();
        else if(skillAction.IsSelecting())
            skillAction.TileSelection();
        else
            TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z).player = gameObject;
        
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

    private void InSkillRangeSelectableTile()
    {
        if(!movementAction.IsUsed() && !attackAction.IsUsed())
        skillAction.GetSelectableTiles();
    }

    public void InSkill1RangeSelectableTile()
    {
        (skillAction as CharacterSkill).skill = classData.firstSkill;
        InSkillRangeSelectableTile();
    }

    public void InSkill2RangeSelectableTile()
    {
        (skillAction as CharacterSkill).skill = classData.secondSkill;
        InSkillRangeSelectableTile();
    }

    public void InSkill3RangeSelectableTile()
    {
        (skillAction as CharacterSkill).skill = classData.thirdSkill;
        InSkillRangeSelectableTile();
    }
}
