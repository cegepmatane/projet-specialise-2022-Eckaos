using System.Collections.Generic;
using UnityEngine;

public class SkillAction : Action
{
    private Tile tileToAttack;
    private Skill skill;

    private bool isSelecting;
    private bool isExecuting;
    public SkillAction(Character character, Skill skill) : base(character)
    {
        this.skill = skill;
    }

    public override void GetSelectableTiles()
    {
        if(!IsValidForUse() || character.movementAction.IsExecuting()) return;
        HighlightTiles(map.GetTileList(), Tile.NORMAL_COLOR);
        selectableTiles = skill.GetSelectableTiles(character);
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
            skill.Execute(character, tileToAttack);
            character.currentActionPoint -= skill.actionPointNeeded;
        }
    }
    public override bool IsValidForUse() => character.currentActionPoint > 0 && character.currentActionPoint >= skill.actionPointNeeded;
    public override bool IsExecuting() => false;
    public override bool IsSelecting() => isSelecting;
    protected override bool IsSelectedTileValid(Tile tile) => selectableTiles != null && tile.IsGround() && selectableTiles.Contains(tile);
}
