using UnityEngine;
using System.Collections.Generic;

public abstract class Skill : ScriptableObject
{
    public string skillName;
    public string description;
    public int actionPointNeeded;
    public int range;
    public abstract void Execute(Character caster, Tile target);
    public virtual List<Tile> GetSelectableTiles(Character caster) => TileMap.GetInstance().AttackBFS(caster.GetCurrentTile(), range);
    protected Character GetCharacterFromTile(Tile tile) {
        if(tile == null || tile.player == null) return null;
        Character c = tile.player.GetComponent<Character>();
        return c;
    } 
}
