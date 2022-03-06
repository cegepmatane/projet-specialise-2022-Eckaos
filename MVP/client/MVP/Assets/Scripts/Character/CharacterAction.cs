using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CharacterAction
{
    protected Character character;
    protected List<Tile> selectableTiles;
    protected bool isSelecting;
    protected bool isExecuting;
    protected TileMap map;

    public CharacterAction(Character character)
    {
        this.character = character;
        this.map = TileMap.GetInstance();
    }

    public bool IsSelecting() => isSelecting;
    public bool IsExecuting() => isExecuting;

    public bool IsUsed() => isSelecting || isExecuting;

    public abstract void GetSelectableTiles();
    public abstract void Execute();
    public void TileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.collider.transform.position;
            Tile tileSelected = map.GetTile((int)targetPosition.x, (int)targetPosition.z);
            if(IsSelectedTileValid(tileSelected))
                SetUpExecution(tileSelected);
            else
                HighlightTiles(selectableTiles, Tile.IN_RANGE_COLOR);
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

    protected Tile GetCurrentTile() => map.GetTile((int)character.transform.position.x, (int)character.transform.position.z);
}
