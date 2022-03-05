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
    
    public static bool operator ==(Tile tile, Tile tile2) => !(tile is null) && !(tile2 is null) && tile.x == tile2.x && tile.z == tile2.z && tile.ground == tile2.ground && tile.player == tile2.player;
    public static bool operator !=(Tile tile, Tile tile2) => tile is null || tile2 is null || tile.x != tile2.x || tile.z != tile2.z || tile.ground != tile2.ground || tile.player != tile2.player;
}
