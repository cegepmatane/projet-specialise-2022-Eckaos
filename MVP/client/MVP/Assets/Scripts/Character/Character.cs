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

    public bool HasAttacked() => attackAction.HasExecuted() || skillAction.HasExecuted();

    public void ResetActions()
    {
        movementAction.Reset();
        attackAction.Reset();
        skillAction.Reset();
    }

    public CharacterAction GetMovementAction() => movementAction;
    public CharacterAction GetAttackAction() => attackAction;
    public CharacterAction GetSkillAction() => skillAction;

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
