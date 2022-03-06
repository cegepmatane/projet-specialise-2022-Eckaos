using UnityEngine;

public abstract class Skill : ScriptableObject
{
    public string skillName;
    public int manaCost;
    public int range;
    public virtual void Execute(Character caster, Tile target)
    {
        caster.classData.manaPoints -= manaCost;
    }
    protected Character GetCharacterFromTile(Tile tile) {
        if(tile == null || tile.player == null) return null;
        Character c = tile.player.GetComponent<Character>();
        return c;
    } 
}
