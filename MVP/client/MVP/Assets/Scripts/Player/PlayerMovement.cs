using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerMovement : MonoBehaviour
{
    public int movementPoint = 5;
    public float moveSpeed = 2;
    public GameObject currentTile;
    private List<GameObject> selectableTiles;
    private Stack<GameObject> path;
    private bool isMoving;

    private readonly Color NORMAL_COLOR = Color.white;
    private readonly Color SELECTABLE_TILE_COLOR = Color.blue;
    private readonly Color PATH_TILE_COLOR = Color.green;

    
    private void Update() {
        if(selectableTiles != null)
            TileSelection();
        if(path!=null)
            MoveTo();
    }

    public GameObject GetCurrentTile()
    {
        return GameObject.FindGameObjectsWithTag(MapGenerator.GROUND_TAG).First(tile => tile.transform.position.x == transform.position.x && tile.transform.position.z == transform.position.z);
    }

    public void GetSelectableTiles()
    {
        if(path != null && path.Count>0) return;
        currentTile = GetCurrentTile();
        selectableTiles = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().BFS(currentTile, movementPoint);
        HighlightTiles(selectableTiles, SELECTABLE_TILE_COLOR);
    }

    void HighlightPathTiles(Stack<GameObject> path)
    {
        if(path == null || path.Count <= 0) return;
        HighlightTiles(path, PATH_TILE_COLOR);
    }

    void HighlightTiles(IEnumerable<GameObject> tiles, Color color)
    {
        foreach (GameObject tile in tiles)
            tile.GetComponent<Renderer>().material.color = color;
    }

    private void MoveTo()
    {
        if(path == null || path.Count <= 0) return;
        GameObject tile = path.Peek();
        Vector3 target = tile.transform.position;
        target.y += GetComponent<Collider>().bounds.extents.y + tile.GetComponent<Collider>().bounds.extents.y;
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
            path.Pop();
        }
    }

    private void TileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
            if(IsSelectedTileValidForMovement(hit.collider.gameObject))
                FindPath(hit.collider.gameObject);
            else
                HighlightTiles(selectableTiles, SELECTABLE_TILE_COLOR);
    }

    void FindPath(GameObject target){
        Stack<GameObject> tempPath = GameObject.Find("MapGenerator").GetComponent<MapGenerator>().AStarSearch(currentTile, target);
        HighlightTiles(selectableTiles, SELECTABLE_TILE_COLOR);
        HighlightPathTiles(tempPath);
        if(Input.GetMouseButtonUp(0))
            SetPath(tempPath);
    }
    void SetPath(Stack<GameObject> tempPath)
    {
        path = tempPath;
        HighlightTiles(selectableTiles, NORMAL_COLOR);
        selectableTiles = null;
    }

    bool IsSelectedTileValidForMovement(GameObject tile)
    {
        return selectableTiles != null && tile.tag == MapGenerator.GROUND_TAG && selectableTiles.Contains(tile);
    }
    
}
