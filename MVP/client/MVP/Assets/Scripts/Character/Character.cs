using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{

    private CharacterAttack attackAction;
    private CharacterMovement movementAction;
    private Class classData;

    // Start is called before the first frame update
    private void Start() {
        var classAssets = Resources.FindObjectsOfTypeAll<Class>();
        TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z).player = gameObject;
        classData = classAssets.GetValue(Random.Range(0, classAssets.Length)) as Class;
        attackAction = new CharacterAttack(gameObject, classData.attackRange);
        movementAction = new CharacterMovement(gameObject, classData.movementPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(movementAction.IsSelecting())
            movementAction.TileSelection();
        else if(attackAction.IsSelecting())
            attackAction.TileSelection();
        
        if(movementAction.IsMoving())
            movementAction.Execute();
        else if(attackAction.IsAttacking())
            attackAction.Execute();
            
    }


    public void InMovingRangeSelectableTile() 
    {
        if(attackAction.IsAttacking() || attackAction.IsSelecting()) return;
        movementAction.GetSelectableTiles();
    }
    public void InAttackRangeSelectableTile() 
    {
        if(movementAction.IsMoving() || movementAction.IsSelecting()) return;
        attackAction.GetAttackableTiles();
    }
}
