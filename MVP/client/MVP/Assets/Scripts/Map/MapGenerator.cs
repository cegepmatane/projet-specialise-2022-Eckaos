using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private bool[,] walls;
    public Material tileMaterial;
    private TileMap tileMap;
    public int xSize = 5;
    public int zSize = 5;
    private int numberOfWalls;
    private GameObject[,] tiles;
    public const string MAP_TAG = "Map";

    void Awake()
    {
        GenerateMap();
    }

    public void GenerateMap()
    {
        DeleteMap();
        GenerateWallPositions();
        GenerateShape();
    }

    public void DeleteMap()
    {
        foreach (GameObject map in GameObject.FindGameObjectsWithTag(MAP_TAG))
        {
            DestroyImmediate(map);
        }
    }

    void GenerateWallPositions()
    {
        int min = (int) Mathf.Log(xSize*zSize, 2);
        int max = xSize <= zSize ? xSize : zSize;
        numberOfWalls = Random.Range(min, max);
        InitWalls();
        for (int i = 0; i < numberOfWalls; i++)
        {
            GenerateWall();
        }
    }

    void GenerateWall()
    {
        int x, z;
        do
        {
            x = Random.Range(0,xSize);
            z = Random.Range(0,zSize);
        } while (walls[x, z]);
        walls[x, z] = true;
    }

    void InitWalls()
    {
        walls = new bool[xSize, zSize];
        for (int x = 0; x < xSize; x++)
        {
            for (int z = 0; z < zSize; z++)
            {
                walls[x, z] = false;
            }
        }
    }

    void GenerateShape()
    {
        GameObject map = new GameObject(MAP_TAG);
        map.tag = MAP_TAG;
        tileMap = TileMap.GetInstance(xSize, zSize);
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
                tileMap.SetTile(x,z, tile);
            }
        }
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
