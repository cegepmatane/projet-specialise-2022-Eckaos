using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public int movementPoint = 5;
    public float moveSpeed = 2;
    private List<Tile> selectableTiles;
    private Stack<Tile> path;
    private bool isMoving;

    private TileMap map;
    private Tile currentTile;

    private readonly Color NORMAL_COLOR = Color.white;
    private readonly Color SELECTABLE_TILE_COLOR = Color.blue;
    private readonly Color PATH_TILE_COLOR = Color.green;

    private void Update() {
        GetCurrentTile();
        if(selectableTiles != null)
            TileSelection();
        if(path!=null)
            MoveTo();
    }

    private Tile GetCurrentTile()
    {
        currentTile = TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z);
        currentTile.player = gameObject;
        return currentTile;
    }

    public void GetSelectableTiles()
    {
        if(path != null && path.Count>0) return;
        selectableTiles = TileMap.GetInstance().MovingBFS(GetCurrentTile(), movementPoint);
        HighlightTiles(selectableTiles, SELECTABLE_TILE_COLOR);
    }

    void HighlightPathTiles(Stack<Tile> path)
    {
        if(path == null || path.Count <= 0) return;
        HighlightTiles(path, PATH_TILE_COLOR);
    }

    void HighlightTiles(IEnumerable<Tile> tiles, Color color)
    {
        foreach (Tile tile in tiles)
            tile.ground.GetComponent<Renderer>().material.color = color;
    }

    private void MoveTo()
    {
        if(path == null || path.Count <= 0) return;
        Tile tile = path.Peek();
        Vector3 target = tile.ground.transform.position;
        target.y += GetComponent<Collider>().bounds.extents.y + tile.ground.GetComponent<Collider>().bounds.extents.y;
        if(Vector3.Distance(transform.position, target) >= 0.05f)
        {
            Vector3 heading = target-transform.position;
            heading.Normalize();
            Vector3 velocity = heading*moveSpeed;
            transform.forward = heading;
            transform.position += velocity * Time.deltaTime;
        }else
        {   
            transform.position = target;
            currentTile.player = null; 
            currentTile = GetCurrentTile();
            path.Pop();
        }
    }

    private void TileSelection()
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
                HighlightTiles(selectableTiles, SELECTABLE_TILE_COLOR);
        }
            
    }

    void FindPath(Tile target){
        Stack<Tile> tempPath = TileMap.GetInstance().AStarSearch(GetCurrentTile(), target);
        HighlightTiles(selectableTiles, SELECTABLE_TILE_COLOR);
        HighlightPathTiles(tempPath);
        if(Input.GetMouseButtonUp(0))
            SetPath(tempPath);
    }
    void SetPath(Stack<Tile> tempPath)
    {
        path = tempPath;
        HighlightTiles(selectableTiles, NORMAL_COLOR);
        selectableTiles = null;
    }

    bool IsSelectedTileValidForMovement(Tile tile)
    {
        return selectableTiles != null && tile.IsWalkable() && selectableTiles.Contains(tile);
    }
    
}
