using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    int[ , ] mapHeights;

    public int xSize = 10;
    public int zSize = 10;
    public int ySize = 3;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GenerateMap();
        CreateShape();
    }


    private void Update() {
        UpdateMesh();
    }

    void CreateShape(){
        vertices = new Vector3[xSize*zSize*8];
        triangles = new int[xSize*zSize*12*3];
        CreateCubes();
    }

    void CreateCubes(){;
        int triangleIndex = 0;
        int verticesIndex = 0;
        for (int z = 0; z < zSize; z++)
            for (int x = 0; x < xSize; x++)
                CreateCube(ref triangleIndex, ref verticesIndex, x, z);
    }

    void CreateCube(ref int triangleIndex, ref int verticesIndex, int x, int z){
        Vector3[] cubeVertices = CreateCubeVertices(x, z, mapHeights[x,z]);
        int[] cubeTriangles = CreateCubeTriangles(x, z, verticesIndex);

        cubeVertices.CopyTo(vertices,verticesIndex);
        cubeTriangles.CopyTo(triangles, triangleIndex);

        triangleIndex += cubeTriangles.Length;
        verticesIndex += cubeVertices.Length;
    }
    
    Vector3[] CreateCubeVertices(int x, int z, int y){
        return new Vector3[]
        {
            new Vector3(x,y,z),
            new Vector3(x+1,y,z),
            new Vector3(x+1,0,z),
            new Vector3(x,0,z),
            new Vector3(x,0,z+1),
            new Vector3(x+1,0,z+1),
            new Vector3(x+1,y,z+1),
            new Vector3(x,y,z+1),
        };
    }

    int[] CreateCubeTriangles(int x, int z, int index){
        return new int[]
        {
            index, index+2, index+1, //face front
	        index, index+3, index+2,
	        index+2, index+3, index+4, //face top
	        index+2, index+4, index+5,
	        index+1, index+2, index+5, //face right
	        index+1, index+5, index+6,
	        index, index+7, index+4, //face left
	        index, index+4, index+3,
	        index+5, index+4, index+7, //face back
	        index+5, index+7, index+6,
	        index, index+6, index+7, //face bottom
	        index, index+1, index+6
        };
    }

    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); 
    }

    private List<(int x, int z)> positionList = new List<(int x, int z)>{(1,0), (-1,0), (0,1), (0,-1)};
    private void GenerateMap(){
        mapHeights = new int[xSize,zSize];
        Queue<(int x, int z)> MarkedPoints = new Queue<(int x, int z)>();
        
        mapHeights[0,0] = Random.Range(2,6);
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
                mapHeights[newPoint.x, newPoint.z] = Random.Range(lastHeight-1 < 1 ? lastHeight : lastHeight-1 , lastHeight+2);
                MarkedPoints.Enqueue(newPoint);
            }
        }
    }
    private bool IsInMap((int x, int z) position){
        return position.x >= 0 && position.x < xSize && position.z >= 0 && position.z < zSize;
    }
}
