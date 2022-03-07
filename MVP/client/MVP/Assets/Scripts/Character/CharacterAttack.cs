using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack : CharacterAction
{
    public int attackRange;
    private Tile tileToAttack;

    public CharacterAttack(Character character, int attackRange = 1) : base(character)
    {
        this.attackRange = character.classData.attackRange;
    }

    public override void GetSelectableTiles()
    {
        HighlightTiles(map.GetTileList(), Tile.NORMAL_COLOR);
        Debug.Log(character);
        selectableTiles = map.AttackBFS(GetCurrentTile(), attackRange);
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        isSelecting = true;
    }

    protected override void SetUpExecution(Tile target)
    {
        tileToAttack = target;
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        HighlightTile(tileToAttack, Tile.ATTACK_COLOR);
        if(Input.GetMouseButtonUp(0))
        {
            tileToAttack = target;
            HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
            selectableTiles = null;
            isSelecting = false;
            isExecuting = true;
        }
    }
    public override void Execute()
    {
        //TODO get component with life et lui enlever un nombre random dans la damageRange
        tileToAttack = null;
        isExecuting = false;
        hasExecuted = true;
    }

    protected override bool IsSelectedTileValid(Tile tile)
    {
        return selectableTiles != null && tile.IsGround() && selectableTiles.Contains(tile);
    }
}
