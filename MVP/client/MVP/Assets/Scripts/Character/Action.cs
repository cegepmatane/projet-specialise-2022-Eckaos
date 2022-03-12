using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    protected List<Tile> selectableTiles;
    protected TileMap map;
    protected IGameClient client;
    
    public void SetTileMap(TileMap tileMap) => map = tileMap; 
    public void SetGameClient(IGameClient gameClient) => client = gameClient;
    public abstract void GetSelectableTiles();
    public virtual void Execute()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.collider.transform.position;
            Tile tileSelected = map.GetTile((int)targetPosition.x, (int)targetPosition.z);
            if(IsSelectedTileValid(tileSelected))
                SetUpExecution(tileSelected);
        }
    }
    protected abstract bool IsSelectedTileValid(Tile tile);
    protected abstract void SetUpExecution(Tile tile);
    protected void HighlightTiles(IEnumerable<Tile> tiles, Color color)
    {
        foreach (Tile tile in tiles)
            tile.ground.GetComponent<Renderer>().material.color = color;
    }
    protected void HighlightTile(Tile tile, Color color)
    {
        tile.ground.GetComponent<Renderer>().material.color = color;
    }
    public Tile GetCurrentTile() => map.GetTile((int)transform.position.x, (int)transform.position.z);
}
