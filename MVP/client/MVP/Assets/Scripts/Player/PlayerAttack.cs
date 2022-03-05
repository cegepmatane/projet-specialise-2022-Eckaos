using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerAttack : MonoBehaviour
{

    public int range;
    public (int min, int max) damageRange;
    private List<Tile> attackableTiles;
    private Tile tileToAttack;
    private readonly Color ATTACKABLE_TILE_COLOR = Color.blue;
    private readonly Color TILE_TO_ATTACK_COLOR = Color.red;
    private readonly Color NORMAL_COlOR = Color.white;
    void Update()
    {
        if(attackableTiles != null)
            TileSelection();
        if(tileToAttack != null)
            Attack();
    }

    public void GetAttackableTiles()
    {
        //if(tileToAttack != null) return;
        attackableTiles = TileMap.GetInstance().AttackBFS(GetCurrentTile(), range);
        HighlightTiles(attackableTiles, ATTACKABLE_TILE_COLOR);
    }
    void TileSelection()
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
                HighlightTiles(attackableTiles, ATTACKABLE_TILE_COLOR);
        }        
    }

    void SetTileToAttack(Tile target)
    {
        tileToAttack = target;
        HighlightTiles(attackableTiles, ATTACKABLE_TILE_COLOR);
        HighlightTile(tileToAttack, TILE_TO_ATTACK_COLOR);
        if(Input.GetMouseButtonUp(0))
        {
            tileToAttack = target;
            HighlightTiles(attackableTiles, NORMAL_COlOR);
            attackableTiles = null;
        }
    }
    void Attack()
    {
        if(tileToAttack == null) return;
        //TODO get component with life et lui enlever un nombre random dans la damageRange
        tileToAttack = null;
    }

    private bool IsSelectableTileValidForAttack(Tile tile)
    {
        return attackableTiles != null && tile.IsGround() && attackableTiles.Contains(tile);
    }

    private Tile GetCurrentTile()
    {
        return TileMap.GetInstance().GetTile((int)transform.position.x, (int)transform.position.z);
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
