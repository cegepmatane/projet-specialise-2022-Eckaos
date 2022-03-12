using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public Material tileMaterial;
    private TileMap tileMap;
    public int xSize;
    public int zSize;
    private GameObject[,] tiles;
    public const string MAP_TAG = "Map";

    public void DeleteMap()
    {
        foreach (GameObject map in GameObject.FindGameObjectsWithTag(MAP_TAG))
        {
            DestroyImmediate(map);
        }
    }

    void GenerateShape(bool[,] walls)
    {
        GameObject map = new GameObject(MAP_TAG);
        map.tag = MAP_TAG;
        tileMap = new TileMap(xSize, zSize);
        for (int x = 0; x < xSize; x++)
        {
            GameObject row = new GameObject("Row"+x);
            row.transform.SetParent(map.transform);
            for (int z = 0; z < zSize; z++)
            {
                GameObject tile;
                if(walls[x,z])
                    tile = GenerateWallTile(x, z);
                else
                    tile = GenerateGroundTile(x, z);

                tile.transform.SetParent(row.transform);
                Renderer tileRenderer = tile.GetComponent<Renderer>();
                tileRenderer.material = GenerateMaterial(tile);
                tileMap.SetGround(x,z, tile);
            }
        }
    }

    public TileMap GenerateMap(bool[,] walls)
    {
        DeleteMap();
        xSize = walls.GetLength(0);
        zSize = walls.GetLength(1);
        GenerateShape(walls);
        return tileMap;
    }

    Material GenerateMaterial(GameObject tile)
    {
        if(tileMaterial == null) return null;
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        Material material = new Material(tileMaterial);
        if(tile.tag == Tile.WALL_TAG) material.color = Color.black;
        if(tile.tag == Tile.GROUND_TAG) material.color = Color.white;
        tileRenderer.material = material;
        return material;
    }
    GameObject GenerateWallTile(int x, int z)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall("+x+","+z+")";
        wall.transform.position = new Vector3(x,0.25f,z);
        wall.transform.localScale = new Vector3(1,1.5f,1);
        wall.tag = Tile.WALL_TAG;
        return wall;
    }

    GameObject GenerateGroundTile(int x, int z)
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Tile("+x+","+z+")";
        ground.transform.position = new Vector3(x,0,z);
        ground.tag = Tile.GROUND_TAG;
        return ground;
    }
}
