using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action
{
    protected Character character;
    protected List<Tile> selectableTiles;
    protected TileMap map;

    public Action(Character character)
    {
        this.character = character;
        this.map = TileMap.GetInstance();
    }
    public abstract bool IsValidForUse();
    public abstract bool IsExecuting();
    public abstract bool IsSelecting();
    public abstract void GetSelectableTiles();
    public virtual void Execute() => TileSelection();
    protected virtual void TileSelection()
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

    public Tile GetCurrentTile() => map.GetTile((int)character.transform.position.x, (int)character.transform.position.z);
}
