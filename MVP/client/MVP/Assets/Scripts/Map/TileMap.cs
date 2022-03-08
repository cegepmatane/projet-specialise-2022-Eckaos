using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class TileMap {
    
    private static TileMap map;
    
    private Tile[,] tileMap;
    private int xSize = 10;
    private int zSize = 10;
    private List<Vector3> directions = new List<Vector3>{Vector3.forward, Vector3.back, Vector3.right, Vector3.left};

    private TileMap(int xSize, int zSize)
    {
        tileMap = new Tile[xSize, zSize];
        this.xSize = xSize;
        this.zSize = zSize;
    }

    public static TileMap GetInstance(int xSize = 10, int zSize = 10)
    {
        if(map == null) map = new TileMap(xSize, zSize);
        return map;
    }

    public Tile GetTile(int x, int z){
        if(!IsValid(x, z)) return null;
        return tileMap[x,z];
    } 

    public List<Tile> GetTileList()
    {
        return tileMap.Cast<Tile>().Where(tile => tile.ground.tag == Tile.GROUND_TAG).ToList();
    }
    public void SetTile(int x, int z, GameObject ground = null, GameObject player = null) => tileMap[x,z] = new Tile(x,z, ground, player);
    
    public void ResetHighlight() 
    {
        foreach(Tile tile in GetTileList())
            tile.ground.GetComponent<Renderer>().material.color = Tile.NORMAL_COLOR;
    }
    public List<Tile> MovingBFS(Tile root, int maxRange)
    {
        List<Tile> selectableTiles = new List<Tile>();
        if(root == null) return selectableTiles;

        Queue<Tile> tileToBeVisited = new Queue<Tile>();
        Queue<int> parentRangeQueue = new Queue<int>();
        bool[,] visitedTiles = new bool[xSize, zSize];

        for(int i = 0; i < xSize; i++)
            for(int j = 0; j < zSize; j++)
                visitedTiles[i,j] = false;

        tileToBeVisited.Enqueue(root);
        parentRangeQueue.Enqueue(0);
        visitedTiles[root.x, root.z] = true;

        while (tileToBeVisited.Count > 0)
        {
            var tile =  tileToBeVisited.Dequeue();
            var range = parentRangeQueue.Dequeue();
            if(range > maxRange) continue;
            for (int i = 0; i < 4; i++)
            {
                int x = (int) (tile.x + directions[i].x);
                int z = (int) (tile.z + directions[i].z);
                int newRange = range+1;
                if(newRange <= maxRange && IsValid(x,z) && !visitedTiles[x,z] && tileMap[x,z].IsWalkable())
                {
                    Tile newTile = tileMap[x,z];
                    selectableTiles.Add(newTile);
                    tileToBeVisited.Enqueue(newTile);
                    parentRangeQueue.Enqueue(newRange);
                    visitedTiles[x,z] = true;
                }  
            }            
        }
        return selectableTiles;
    }

    public List<Tile> AttackBFS(Tile root, int maxRange)
    {
        List<Tile> selectableTiles = new List<Tile>();
        if(root == null) return selectableTiles;

        Queue<Tile> tileToBeVisited = new Queue<Tile>();
        Queue<int> parentRangeQueue = new Queue<int>();
        bool[,] visitedTiles = new bool[xSize, zSize];

        for(int i = 0; i < xSize; i++)
            for(int j = 0; j < zSize; j++)
                visitedTiles[i,j] = false;

        tileToBeVisited.Enqueue(root);
        parentRangeQueue.Enqueue(0);
        visitedTiles[root.x, root.z] = true;

        while (tileToBeVisited.Count > 0)
        {
            var tile =  tileToBeVisited.Dequeue();
            var range = parentRangeQueue.Dequeue();
            if(range > maxRange) continue;
            for (int i = 0; i < 4; i++)
            {
                int x = (int) (tile.x + directions[i].x);
                int z = (int) (tile.z + directions[i].z);
                int newRange = range+1;
                if(newRange <= maxRange && IsValid(x,z) && !visitedTiles[x,z] && tileMap[x,z].IsGround())
                {
                    Tile newTile = tileMap[x,z];
                    selectableTiles.Add(newTile);
                    tileToBeVisited.Enqueue(newTile);
                    parentRangeQueue.Enqueue(newRange);
                    visitedTiles[x,z] = true;
                }  
            }            
        }
        return selectableTiles;
    }

    private bool IsValid(int x, int z)
    {
        return x>=0 && x<xSize && z>=0 && z<zSize;
    }


    public Stack<Tile> AStarSearch(Tile start, Tile dest)
    {
        if(!isValidForAStarSearch(start, dest)) return null;

        bool[,] closedList = new bool[xSize, zSize];
        Cell[,] cellsDetails = new Cell[xSize,zSize];
        int i, j;
        InitAStarSearch(ref closedList, ref cellsDetails);   

        i = (int)start.x;
        j = (int)start.z;
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
                if(IsValid(iNew,jNew))
                {
                    if(tileMap[iNew,jNew] == dest)
                    {
                        cellsDetails[iNew, jNew].i = i;
                        cellsDetails[iNew, jNew].j = j;
                        return TracePath(cellsDetails, dest);
                    }
                    else if(closedList[iNew,jNew] == false && tileMap[iNew, jNew].IsWalkable())
                    {
                        gNew = cellsDetails[i, j].g + 1;
                        hNew = (float)CalculateHValue(iNew, jNew, (dest.x, dest.z));
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
        return new Stack<Tile>();
    }
    struct Cell
    {
        public int i,j;
        public float f,g,h;
    }

    private bool isValidForAStarSearch(Tile start, Tile dest) => IsValid(start.x, start.z) && IsValid(dest.x, dest.z) && dest.IsWalkable() && start != dest;
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

    private void ChangeCellValue(ref Cell cell, int i, int j, float f, float g, float h)
    {
        cell.i = i;
        cell.j = j;
        cell.f = f;
        cell.g = g;
        cell.h = h;
    }

    private Stack<Tile> TracePath(Cell[,] cells, Tile dest)
    {
        int x = (int)dest.x;
        int z = (int)dest.z;
        Stack<Tile> path = new Stack<Tile>();
        while (!(cells[x,z].i == x && cells[x,z].j == z))
        {
           path.Push(tileMap[x,z]);
           int temp_x = cells[x,z].i;
           int temp_z = cells[x,z].j;
           x = temp_x;
           z = temp_z;
        }
        return path;
    }

    private int CalculateHValue(int x, int z, (int x, int z) dest) => Mathf.Abs(x-dest.x)+Mathf.Abs(z-dest.z);
}
