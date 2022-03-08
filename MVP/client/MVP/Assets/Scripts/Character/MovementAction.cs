using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MovementAction : Action
{
    public float moveSpeed = 6;
    private Stack<Tile> path;

    private bool isExecuting;

    public MovementAction(Character character) : base(character) {}

    public override void Execute()
    {
        base.Execute();
        Move();
    }

    protected override void TileSelection()
    {
        GetSelectableTiles();
        base.TileSelection();
    }
    public override void GetSelectableTiles()
    {
        if(path != null && path.Count>0) return;
        selectableTiles = map.MovingBFS(GetCurrentTile(), character.currentMovementPoint);
        HighlightTiles(selectableTiles, Color.green);
    }

    private void Move()
    {
        if(path == null || path.Count <= 0){
            isExecuting = false;
            return;
        }
        GetCurrentTile().player = null;
        Tile tile = path.Peek();
        Vector3 target = tile.ground.transform.position;
        target.y += character.GetComponent<Collider>().bounds.extents.y + tile.ground.GetComponent<Collider>().bounds.extents.y;
        if(Vector3.Distance(character.transform.position, target) >= 0.05f)
        {
            Vector3 heading = target-character.transform.position;
            heading.Normalize();
            Vector3 velocity = heading*moveSpeed;
            character.transform.forward = heading;
            character.transform.position += velocity * Time.deltaTime;
        }else
        {   
            character.transform.position = target;
            GetCurrentTile().player = character.gameObject;
            path.Pop();
        }
    }

    protected override void SetUpExecution(Tile target){
        Stack<Tile> tempPath = map.AStarSearch(GetCurrentTile(), target);
        HighlightTiles(selectableTiles, Color.green);
        HighlightTiles(tempPath, Color.grey);
        if(Input.GetMouseButtonUp(0))
            SetPath(tempPath);
    }

    void SetPath(Stack<Tile> tempPath)
    {
        path = tempPath;
        character.currentMovementPoint -= path.Count();
        HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
        selectableTiles = null;
        isExecuting = true;
    }

    public override bool IsValidForUse() =>  character.currentMovementPoint >= 0;
    public override bool IsExecuting() => isExecuting;
    public override bool IsSelecting() => false;
    protected override bool IsSelectedTileValid(Tile tile) => selectableTiles != null && tile.IsWalkable() && selectableTiles.Contains(tile);
}
