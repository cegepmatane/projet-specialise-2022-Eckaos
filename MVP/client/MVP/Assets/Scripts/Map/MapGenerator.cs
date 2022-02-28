using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int[ , ] mapHeights;

    public int xSize = 10;
    public int zSize = 10;

    public int maxHeight = 4;
    private GameObject map;
    public Material tileMaterial;

    private List<GameObject> cubes;

    // Start is called before the first frame update
    void Start()
    {
        GenerateMap();
    }
    
    public void GenerateMap()
    {
        DeleteMap();
        GenerateMapHeight();
        CreateMap();
    }

    public void DeleteMap()
    {
        foreach (GameObject mapObject in GameObject.FindGameObjectsWithTag("Map"))
        {
            DestroyImmediate(mapObject);
        }
    }

    private void CreateMap()
    {
        cubes = new List<GameObject>();
        map = new GameObject("Map");
        map.tag = "Map";
        for (int i = 0; i < xSize; i++)
        {
            CreateRow(i);
        }
    }

    private void CreateRow(int i){
        GameObject row = new GameObject("Row"+i);
        row.transform.SetParent(map.transform);
        for (int j = 0; j < zSize; j++)
        {
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.position = new Vector3(i,mapHeights[i,j],j);
            cube.transform.SetParent(row.transform);
            cube.tag = "Tile";
            cubes.Add(cube);
            if(tileMaterial != null) cube.GetComponent<Renderer>().material = tileMaterial;
        }
    }

    private List<(int x, int z)> positionList = new List<(int x, int z)>{(1,0), (-1,0), (0,1), (0,-1)};
    private void GenerateMapHeight(){
        mapHeights = new int[xSize,zSize];
        Queue<(int x, int z)> MarkedPoints = new Queue<(int x, int z)>();
        
        mapHeights[(xSize+1)/2, (zSize+1)/2] = Random.Range(2,6);
        MarkedPoints.Enqueue(((xSize+1)/2,(zSize+1)/2));

        while (MarkedPoints.Count > 0)
        {
            (int x, int z) actualPoints = MarkedPoints.Dequeue();
            foreach (var position in positionList)
            {
                (int x, int z) newPoint = (actualPoints.x+position.x, actualPoints.z+position.z);
                if(!IsInMap(newPoint) || mapHeights[newPoint.x, newPoint.z] != 0)
                    continue;
                int lastHeight = mapHeights[actualPoints.x, actualPoints.z];
                mapHeights[newPoint.x, newPoint.z] = Random.Range(lastHeight-1 < 1 ? lastHeight : lastHeight-1 , lastHeight+1 > maxHeight? maxHeight+1 : lastHeight+2);
                MarkedPoints.Enqueue(newPoint);
            }
        }
    }
    private bool IsInMap((int x, int z) position){
        return position.x >= 0 && position.x < xSize && position.z >= 0 && position.z < zSize;
    }
}
