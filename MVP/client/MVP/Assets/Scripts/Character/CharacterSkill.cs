using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSkill : CharacterAction
{
    public Skill skill;
    private Tile tileToAttack;
    public CharacterSkill(Character character) :base(character){}


    public override void Execute()
    {
        skill.Execute(character, tileToAttack);
        tileToAttack = null;
        isExecuting = false;
        hasExecuted = true;
    }

    public override void GetSelectableTiles()
    {
        HighlightTiles(map.GetTileList(), Tile.NORMAL_COLOR);
        selectableTiles = map.AttackBFS(GetCurrentTile(), skill.range);
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

    protected override bool IsSelectedTileValid(Tile tile)
    {
        return selectableTiles != null && tile.IsGround() && selectableTiles.Contains(tile);
    }
}
