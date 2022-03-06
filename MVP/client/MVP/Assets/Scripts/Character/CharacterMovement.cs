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
    }
    public override void GetSelectableTiles()
    {
        if(path != null && path.Count>0) return;
        currentTile = GetCurrentTile();
        selectableTiles = map.MovingBFS(currentTile, movementPoint);
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        isSelecting = true;
    }

    public override void Execute()
    {
        if(path == null || path.Count <= 0)
        {
            currentTile.player = null;
            currentTile = GetCurrentTile();
            currentTile.player = character.gameObject;
            isExecuting = false;
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
            path.Pop();
        }
    }

    protected override void SetUpExecution(Tile target){
        Stack<Tile> tempPath = map.AStarSearch(GetCurrentTile(), target);
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        HighlightTiles(tempPath, Tile.PATH_COLOR);
        if(Input.GetMouseButtonUp(0))
            SetPath(tempPath);
    }

    void SetPath(Stack<Tile> tempPath)
    {
        path = tempPath;
        HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
        selectableTiles = null;
        isSelecting = false;
        isExecuting = true;
    }


    protected override bool IsSelectedTileValid(Tile tile)
    {
        return selectableTiles != null && tile.IsWalkable() && selectableTiles.Contains(tile);
    }
}
