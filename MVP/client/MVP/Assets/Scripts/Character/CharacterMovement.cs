using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CharacterMovement
{
    public int movementPoint;
    public float moveSpeed = 6;
    private List<Tile> selectableTiles;
    private Stack<Tile> path;
    

    private TileMap map;
    private Tile currentTile;

    private GameObject character;
    private bool isSelecting;
    private bool isMoving;

    public CharacterMovement(GameObject character, int movementPoint = 5)
    {
        this.character = character;
        this.movementPoint = movementPoint;
    }

    public bool IsSelecting() => isSelecting;
    public bool IsMoving() => isMoving;
    private Tile GetCurrentTile()
    {
        currentTile = TileMap.GetInstance().GetTile((int)character.transform.position.x, (int)character.transform.position.z);
        currentTile.player = character;
        return currentTile;
    }

    public void GetSelectableTiles()
    {
        if(path != null && path.Count>0) return;
        selectableTiles = TileMap.GetInstance().MovingBFS(GetCurrentTile(), movementPoint);
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        isSelecting = true;
    }

    void HighlightPathTiles(Stack<Tile> path)
    {
        if(path == null || path.Count <= 0) return;
        HighlightTiles(path, Tile.PATH_COLOR);
    }

    void HighlightTiles(IEnumerable<Tile> tiles, Color color)
    {
        foreach (Tile tile in tiles)
            tile.ground.GetComponent<Renderer>().material.color = color;
    }

    public void Execute()
    {
        if(path == null || path.Count <= 0)
        {
            isMoving = false;
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
            path.Pop();
        }
    }

    public void TileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.collider.transform.position;
            Tile target = TileMap.GetInstance().GetTile((int)targetPosition.x ,(int)targetPosition.z);
            if(IsSelectedTileValidForMovement(target))
                FindPath(target);
            else
                HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        }
    }

    void FindPath(Tile target){
        Stack<Tile> tempPath = TileMap.GetInstance().AStarSearch(GetCurrentTile(), target);
        HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
        HighlightPathTiles(tempPath);
        if(Input.GetMouseButtonUp(0))
            SetPath(tempPath);
    }
    void SetPath(Stack<Tile> tempPath)
    {
        path = tempPath;
        HighlightTiles(selectableTiles, Tile.NORMAL_COLOR);
        selectableTiles = null;
        isSelecting = false;
        isMoving = true;
    }

    bool IsSelectedTileValidForMovement(Tile tile)
    {
        return selectableTiles != null && tile.IsWalkable() && selectableTiles.Contains(tile);
    }
    
}
