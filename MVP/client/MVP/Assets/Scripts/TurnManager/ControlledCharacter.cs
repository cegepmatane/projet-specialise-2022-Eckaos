using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlledCharacter : MonoBehaviour
{
    private Character currentCharacter;
    public TurnManager turnManager;

    private CharacterAction attackAction;
    private CharacterAction movementAction;
    private CharacterAction skillAction;



    // Update is called once per frame
    void Update()
    {
        if(currentCharacter == null)
            ChangeCurrentCharacter();

        if(attackAction.IsSelecting())
            attackAction.TileSelection();
        else if(skillAction.IsSelecting())
            skillAction.TileSelection();
        else if(!movementAction.HasExecuted())
            movementAction.TileSelection();

        if(movementAction.IsExecuting())
            movementAction.Execute();
        else if(attackAction.IsExecuting())
            attackAction.Execute();
        else if(skillAction.IsExecuting())
            skillAction.Execute();
    }

    private void ChangeCurrentCharacter()
    {

        if(currentCharacter != null)currentCharacter.ResetActions();
        currentCharacter = turnManager.GetTurn();
        attackAction = currentCharacter.GetAttackAction();
        movementAction = currentCharacter.GetMovementAction();
        skillAction = currentCharacter.GetSkillAction();
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
        (skillAction as CharacterSkill).skill = currentCharacter.classData.firstSkill;
        InSkillRangeSelectableTile();
    }

    public void InSkill2RangeSelectableTile()
    {
        (skillAction as CharacterSkill).skill = currentCharacter.classData.secondSkill;
        InSkillRangeSelectableTile();
    }

    public void InSkill3RangeSelectableTile()
    {
        (skillAction as CharacterSkill).skill = currentCharacter.classData.thirdSkill;
        InSkillRangeSelectableTile();
    }
    public void NextTurn() => ChangeCurrentCharacter();
}
