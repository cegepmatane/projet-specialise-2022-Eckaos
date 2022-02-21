using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MapGenerator : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;
    int[] triangles;

    public int xSize = 10;
    public int zSize = 10;
    public int ySize = 3;
    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        CreateShape();
    }


    private void Update() {
        UpdateMesh();
    }

    void CreateShape(){
        vertices = new Vector3[(xSize+1)*(zSize+1)*4];
        CreateVertices();
        CreateTopTriangles();
    }

    void CreateVertices(){
        int i = -1;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                vertices[++i] = new Vector3(x,0,z);
            }
        }
    }
    
    void CreateTriangles(){
        //2 Triangles en haut
        //2 Triangles pour chaque cotes
        
        
        CreateTopTriangles();
        //CreateSideTriangles();
    }

    void CreateTopTriangles(){
        triangles = new int[xSize * zSize * 6];
        int tris = 0;
        int vert = 0;
        for (int z = 0; z < zSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert;
                triangles[tris + 4] = vert + xSize;
                triangles[tris + 5] = vert + xSize + 1;
                vert++;
                tris+=6;
            }
        }
    }

    void CreateSideTriangles(){

    }

    void CreateFirstRectangleVertices(ref int i){
        vertices[++i] = new Vector3(0, 0, 0);
        vertices[++i] = new Vector3(1, 0, 0);
        vertices[++i] = new Vector3(0, 0, 1);
        vertices[++i] = new Vector3(1, 0, 1);
    }
    void CreateRectangleVertices(int x, int z, ref int i){
        float previousHeight = vertices[i-1].y;
        float newHeight = (int) Random.Range(previousHeight-1, previousHeight+2);
        vertices[++i] = new Vector3(x, newHeight, z);
        vertices[++i] = new Vector3(x+1, newHeight, z);
        vertices[++i] = new Vector3(x, newHeight, z+1);
        vertices[++i] = new Vector3(x+1, newHeight, z+1);
    }
    void UpdateMesh(){
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals(); 
    }

    private void OnDrawGizmos() {
        if(vertices == null)
            return;
        
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }
}
