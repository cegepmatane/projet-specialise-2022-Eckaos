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
}
