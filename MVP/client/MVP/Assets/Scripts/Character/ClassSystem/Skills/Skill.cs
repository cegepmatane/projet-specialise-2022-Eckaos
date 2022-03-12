using UnityEngine;
using System.Collections.Generic;

public abstract class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int actionPointNeeded;
    public int range;
    public abstract void Execute(Character caster, Tile target);
    public virtual List<Tile> GetSelectableTiles(Tile casterTile, TileMap map) => map.AttackBFS(casterTile, range);
    protected Character GetCharacterFromTile(Tile tile) {
        if(tile == null || tile.player == null) return null;
        Character c = tile.player.GetComponent<Character>();
        return c;
    } 
}
