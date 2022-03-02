using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    private bool[,] walls;
    public Material tileMaterial;

    public int xSize = 5;
    public int zSize = 5;
    private int numberOfWalls;

    private GameObject[,] tiles;

    public const string GROUND_TAG = "Ground";
    public const string WALL_TAG = "Wall";
    public const string MAP_TAG = "Map";

    void Start()
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
        tiles = new GameObject[xSize,zSize];
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
            }
        }
    }

    Material GenerateMaterial(GameObject tile)
    {
        if(tileMaterial == null) return null;
        Renderer tileRenderer = tile.GetComponent<Renderer>();
        Material material = new Material(tileMaterial);
        if(tile.tag == WALL_TAG) material.color = Color.black;
        if(tile.tag == GROUND_TAG) material.color = Color.white;
        tileRenderer.material = material;
        return material;
    }
    GameObject GenerateWallTile(int x, int z)
    {
        GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
        wall.name = "Wall("+x+","+z+")";
        wall.transform.position = new Vector3(x,0.25f,z);
        wall.transform.localScale = new Vector3(1,1.5f,1);
        wall.tag = WALL_TAG;
        tiles[x,z] = wall;
        return wall;
    }

    GameObject GenerateGroundTile(int x, int z)
    {
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Cube);
        ground.name = "Tile("+x+","+z+")";
        ground.transform.position = new Vector3(x,0,z);
        ground.tag = GROUND_TAG;
        tiles[x,z] = ground;
        return ground;
    }

    private List<Vector3> directions = new List<Vector3>{Vector3.forward, Vector3.back, Vector3.right, Vector3.left};
    public List<GameObject> BFS(GameObject root, int maxRange)
    {
        List<GameObject> selectableTile = new List<GameObject>();
        Queue<GameObject> tilesToBeVisited = new Queue<GameObject>();
        Queue<int> parentRangeQueue = new Queue<int>();
        bool[,] visitedTiles = new bool[xSize, zSize];
        for(int i = 0; i < xSize; i++)
            for(int j = 0; j < zSize; j++)
                visitedTiles[i,j] = false;

        tilesToBeVisited.Enqueue(root);
        parentRangeQueue.Enqueue(0);
        visitedTiles[(int)root.transform.position.x, (int)root.transform.position.z] = true;

        while (tilesToBeVisited.Count > 0)
        {
            var tile =  tilesToBeVisited.Dequeue();
            var range = parentRangeQueue.Dequeue();
            if(range > maxRange) continue;
            for (int i = 0; i < 4; i++)
            {
                int x = (int) (tile.transform.position.x + directions[i].x);
                int z = (int) (tile.transform.position.z + directions[i].z);
                int newRange = range+1;
                if(newRange <= maxRange && isValid(x,z) && !visitedTiles[x,z]&& tiles[x,z].tag == GROUND_TAG)
                {
                    GameObject newTile = tiles[x,z];
                    if(newTile == null) continue;
                    selectableTile.Add(newTile);
                    tilesToBeVisited.Enqueue(newTile);
                    parentRangeQueue.Enqueue(newRange);
                    visitedTiles[x,z] = true;
                }  
            }            
        }
        return selectableTile;
    }

    public Stack<GameObject> AStarSearch(GameObject start, GameObject dest)
    {
        if(!isValidForAStarSearch(start, dest)) return null;

        bool[,] closedList = new bool[xSize, zSize];
        Cell[,] cellsDetails = new Cell[xSize,zSize];
        int i, j;
        InitAStarSearch(ref closedList, ref cellsDetails);   

        i = (int)start.transform.position.x;
        j = (int)start.transform.position.z;
        ChangeCellValue(ref cellsDetails[i,j], i, j, 0,0,0);

        Queue<(float f,(int x, int z) pos)> openList = new Queue<(float f, (int x, int z) pos)>();
        openList.Enqueue((0, (i,j)));

        while(openList.Count > 0)
        {
            var p = openList.Dequeue();
            i = p.pos.x;
            j = p.pos.z;
            closedList[i,j] = true;

            float fNew, gNew, hNew;
            foreach (Vector3 direction in directions)
            {
                int iNew = (int)(i+direction.x);
                int jNew = (int)(j+direction.z);
                if(isValid(iNew,jNew))
                {
                    if(tiles[iNew,jNew] == dest)
                    {
                        cellsDetails[iNew, jNew].i = i;
                        cellsDetails[iNew, jNew].j = j;
                        return TracePath(cellsDetails, dest);
                    }
                    else if(closedList[iNew,jNew] == false && IsUnblocked(tiles[iNew, jNew]))
                    {
                        gNew = cellsDetails[i, j].g + 1;
                        hNew = (float)CalculateHValue(iNew, jNew, ((int)dest.transform.position.x, (int)dest.transform.position.z));
                        fNew = gNew + hNew;
                        if(cellsDetails[iNew, jNew].f == Mathf.Infinity || cellsDetails[iNew, jNew].f > fNew)
                        {
                            openList.Enqueue((fNew,(iNew, jNew)));
                            ChangeCellValue(ref cellsDetails[iNew, jNew], i, j, fNew, gNew, hNew);
                        }
                    }
                }
            }
        }
        return new Stack<GameObject>();
    }

    private bool IsUnblocked(GameObject tile)
    {
        return tile != null && tile.tag == GROUND_TAG;
    }

    private void InitAStarSearch(ref bool[,] closedList, ref Cell[,] cellsDetails)
    {
        for (int i = 0; i < xSize; i++)
        {
            for (int j = 0; j < zSize; j++)
            {
                closedList[i,j] = false;
                ChangeCellValue(ref cellsDetails[i,j], -1, -1, Mathf.Infinity, Mathf.Infinity, Mathf.Infinity);
            }
                
        } 
    }

    private bool isValidForAStarSearch(GameObject start, GameObject dest)
    {
        return isValid((int)start.transform.position.x, (int)start.transform.position.z) && isValid((int)dest.transform.position.x, (int)dest.transform.position.z)
            && start.tag == GROUND_TAG && dest.tag == GROUND_TAG
            && start != dest;
    }

    private void ChangeCellValue(ref Cell cell, int i, int j, float f, float g, float h)
    {
        cell.i = i;
        cell.j = j;
        cell.f = f;
        cell.g = g;
        cell.h = h;
    }

    struct Cell
    {
        public int i,j;
        public float f,g,h;
    }

    private bool isValid(int x, int z)
    {
        return x>=0 && x<xSize && z>=0 && z<zSize;
    }

    private Stack<GameObject> TracePath(Cell[,] cells, GameObject dest)
    {
        int x = (int)dest.transform.position.x;
        int z = (int)dest.transform.position.z;
        Stack<GameObject> path = new Stack<GameObject>();
        while (!(cells[x,z].i == x && cells[x,z].j == z))
        {
           path.Push(tiles[x,z]);
           int temp_x = cells[x,z].i;
           int temp_z = cells[x,z].j;
           x = temp_x;
           z = temp_z;
        }
        return path;
    }

    private int CalculateHValue(int x, int z, (int x, int z) dest)
    {
        return Mathf.Abs(x-dest.x)+Mathf.Abs(z-dest.z);
    }
}
