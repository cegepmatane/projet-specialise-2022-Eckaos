using System.Collections.Generic;
using UnityEngine;

public class SkillAction : Action
{
    private Tile tileToAttack;
    private Skill skill;
    public MovementAction movement;
    private bool isSelecting = false;
    public override void GetSelectableTiles()
    {
        if(movement.IsExecuting()) return;
        map.ResetHighlight();
        if(GetComponent<Character>().currentActionPoint - skill.actionPointNeeded  < 0) return;
        selectableTiles = skill.GetSelectableTiles(GetCurrentTile(), map);
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        isSelecting = true;
    }

    public void ResetHighlight() => map.ResetHighlight();

    public override void Execute()
    {
        if(isSelecting == false) return;
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        base.Execute();
    }
    protected override void SetUpExecution(Tile target)
    {
        HighlightTile(target, Tile.ATTACK_COLOR);
        if(Input.GetMouseButtonUp(0))
        {
            HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
            Character caster = GetComponent<Character>();
            skill.Execute(caster, target);
            caster.currentActionPoint -= skill.actionPointNeeded;
            if(target.player != null) 
                client.SendAction(new CharacterMessage(target.x, target.z, target.player.GetComponent<Character>().currentLifePoints));
            isSelecting = false;
        }
    }

    public Skill GetSkill() => skill;
    public void SetSkill(Skill skill) => this.skill = skill;
    public bool IsSelecting() => isSelecting;
    public void Selecting(bool isSelecting) => this.isSelecting = isSelecting;
    protected override bool IsSelectedTileValid(Tile tile) => selectableTiles != null && tile.IsGround() && selectableTiles.Contains(tile);
}
