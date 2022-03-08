using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterMovement : CharacterAction
{
    public int movementPoint;
    public float moveSpeed = 6;
    private Stack<Tile> path;
    private Tile currentTile;

    public CharacterMovement(Character character, int movementPoint = 5) : base(character)
    {
        this.movementPoint = movementPoint;
        isSelecting = false;
    }
    public override void GetSelectableTiles()
    {
        if(path != null && path.Count>0) return;
        currentTile = GetCurrentTile();
        selectableTiles = map.MovingBFS(currentTile, movementPoint);
        HighlightTiles(selectableTiles, Color.green);
    }

    public override void Execute()
    {
        if(path == null || path.Count <= 0)
        {
            isExecuting = false;
            hasExecuted = true;
            return;
        }
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
            currentTile.player = null;
            currentTile = GetCurrentTile();
            currentTile.player = character.gameObject;
            path.Pop();
        }
    }

    public override void TileSelection()
    {
        if(hasExecuted || isExecuting) return;
        GetSelectableTiles();
        base.TileSelection();
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
        HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
        selectableTiles = null;
        isExecuting = true;
    }


    protected override bool IsSelectedTileValid(Tile tile)
    {
        return selectableTiles != null && tile.IsWalkable() && selectableTiles.Contains(tile);
    }
}
