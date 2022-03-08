using UnityEngine;

public class Tile
{
    public GameObject ground;
    public GameObject player;
    public int x;
    public int z;

    public static readonly Color IN_RANGE_COLOR = Color.blue;
    public static readonly Color NORMAL_COLOR = Color.white;
    public static readonly Color ATTACK_COLOR = Color.red;
    public static readonly Color PATH_COLOR = Color.green;

    public static readonly string GROUND_TAG = "Ground";
    public static readonly string WALL_TAG = "Wall";

    public Tile(int x, int z, GameObject ground, GameObject player)
    {
        this.ground = ground;
        this.player = player;
        this.x = x;
        this.z = z;
    }

    public bool IsWalkable() => IsGround() && player == null;
    public bool IsGround() => ground.tag == GROUND_TAG;

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())  return false;
        Tile tile2 = obj as Tile;
        return this.x == tile2.x && this.z == tile2.z && this.ground == tile2.ground && this.player == tile2.player;
    }
    
    public override int GetHashCode() => base.GetHashCode();

}
