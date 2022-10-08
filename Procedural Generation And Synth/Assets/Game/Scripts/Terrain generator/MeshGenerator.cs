using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerator
{
    public static MeshData GenrateTerrainMesh(float[,] heightMap, AnimationCurve curve, float heightMultiplier, int levelOfDetail)
    {
        int width = heightMap.GetLength(0);
        int height = heightMap.GetLength(1);
        float topLeftX = (width - 1) / - 2f;
        float topLeftZ = (height - 1) / 2f;
        int vertexIndex = 0;
        int increment = (levelOfDetail <= 0) ? 1 : levelOfDetail * 2;
        int verteciesPerLine = (width - 1) / increment + 1;
        MeshData meshData = new MeshData(verteciesPerLine, verteciesPerLine);

        for (int y = 0; y < height; y += increment)
        {
            for (int x = 0; x < width; x += increment)
            {
                meshData.vertecies[vertexIndex] = new Vector3(topLeftX + x, curve.Evaluate(heightMap[x, y]) * heightMultiplier, topLeftZ - y);
                meshData.uvs[vertexIndex] = new Vector2(x / (float)width, y / (float)height);

                if (x < width - 1 && y < height - 1) 
                {
                    meshData.AddTriangle(vertexIndex, vertexIndex + verteciesPerLine + 1, vertexIndex + verteciesPerLine);
                    meshData.AddTriangle(vertexIndex + verteciesPerLine + 1, vertexIndex, vertexIndex + 1);
                }
                vertexIndex++;
            }
        }

        return meshData;
    }
}

public class MeshData
{
    public Vector3[] vertecies;
    public int[] triangles;
    public Vector2[] uvs;

    private int triangleIndex;

    public MeshData(int width, int height)
    {
        int size = width * height;
        vertecies = new Vector3[size];
        uvs = new Vector2[size];
        triangles = new int[(width - 1) * (height - 1) * 6];
    }

    public void AddTriangle(int a, int b, int c)
    {
        triangles[triangleIndex] = a;
        triangles[triangleIndex + 1] = b;
        triangles[triangleIndex + 2] = c;
        triangleIndex += 3;
    }

    public Mesh CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertecies;
        mesh.uv = uvs;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
}
