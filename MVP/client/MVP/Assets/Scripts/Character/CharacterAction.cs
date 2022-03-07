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

    protected bool hasExecuted;

    public CharacterAction(Character character)
    {
        this.character = character;
        this.map = TileMap.GetInstance();
        hasExecuted = false;
    }

    public bool IsSelecting() => isSelecting;
    public bool IsExecuting() => isExecuting;
    public bool HasExecuted() => hasExecuted;
    public void Reset() => hasExecuted = false;

    public bool IsUsed() => isSelecting || isExecuting;

    public abstract void GetSelectableTiles();
    public abstract void Execute();
    public virtual void TileSelection()
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
