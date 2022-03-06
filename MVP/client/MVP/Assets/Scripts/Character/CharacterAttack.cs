using System.Collections.Generic;
using UnityEngine;

public class CharacterAttack
{
    public int attackRange;
    private List<Tile> attackableTiles;
    private Tile tileToAttack;
    private GameObject character;
    private bool isSelecting;
    private bool isAttacking;


    public CharacterAttack(GameObject character, int attackRange = 1)
    {
        this.character = character;
        this.attackRange = attackRange;
    }

    public bool IsSelecting() => isSelecting;
    public bool IsAttacking() => isAttacking;
    public void GetAttackableTiles()
    {
        if(tileToAttack == null) return;
        attackableTiles = TileMap.GetInstance().AttackBFS(GetCurrentTile(), attackRange);
        HighlightTiles(attackableTiles, Tile.IN_RANGE_COLOR);
        isSelecting = true;
    }
    public void TileSelection()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit))
        {
            Vector3 targetPosition = hit.collider.transform.position;
            Tile target = TileMap.GetInstance().GetTile((int)targetPosition.x, (int)targetPosition.z);
            if(IsSelectableTileValidForAttack(target))
                SetTileToAttack(target);
            else
                HighlightTiles(attackableTiles, Tile.IN_RANGE_COLOR);
        }        
    }

    void SetTileToAttack(Tile target)
    {
        tileToAttack = target;
        HighlightTiles(attackableTiles, Tile.IN_RANGE_COLOR);
        HighlightTile(tileToAttack, Tile.ATTACK_COLOR);
        if(Input.GetMouseButtonUp(0))
        {
            tileToAttack = target;
            HighlightTiles(attackableTiles, Tile.NORMAL_COLOR);
            attackableTiles = null;
            isSelecting = false;
            isAttacking = true;
        }
    }
    public void Execute()
    {
        if(tileToAttack == null) return;
        //TODO get component with life et lui enlever un nombre random dans la damageRange
        tileToAttack = null;
        isAttacking = false;
    }

    private bool IsSelectableTileValidForAttack(Tile tile)
    {
        return attackableTiles != null && tile.IsGround() && attackableTiles.Contains(tile);
    }

    private Tile GetCurrentTile()
    {
        return TileMap.GetInstance().GetTile((int)character.transform.position.x, (int)character.transform.position.z);
    }

    void HighlightTiles(IEnumerable<Tile> tiles, Color color)
    {
        foreach (Tile tile in tiles)
            tile.ground.GetComponent<Renderer>().material.color = color;
    }

    void HighlightTile(Tile tile, Color color)
    {
        tile.ground.GetComponent<Renderer>().material.color = color;
    }
}
