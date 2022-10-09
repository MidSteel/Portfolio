using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private List<Vector3> _vertecies = new List<Vector3>();
    [SerializeField] private List<int> _triangleIndex = new List<int>();
    [SerializeField] private Transform _parentObject = null;
    [SerializeField] private List<Vector3> _points = new List<Vector3>();
    [SerializeField] private List<Vector3> _vertPoints = new List<Vector3>();
    [SerializeField] private List<int> _meshTriangles = new List<int>();

    public List<Vector3> Vertecies { get { return _vertecies; } }
    public List<Vector3> Points { get { return _points; } }
    public List<Vector3> VertPoints { get { return _vertPoints; } }
    public List<int> TriangleIndex { get { return _triangleIndex; } }
    public int GetNextPointIndex { get { if (_triangleIndex.Count <= 0) { return 0; } else return Mathf.Max(_triangleIndex.ToArray()) + 1; } }

    [ContextMenu("ADD a Triangle")]
    public void AddATriangle()
    {
        Vector3[] positions = new Vector3[3];

        positions[0] = new Vector3(-10f, 0f, 0f);
        positions[1] = new Vector3(10f, 0f, 0f);
        positions[2] = new Vector3(5f, 0f, -10f);

        for (int i = 0; i < 3; i++)
        {
            _vertPoints.Add(positions[i]);  
        }
    }

    [ContextMenu("GenerateMesh")]
    public void MeshGeneration()
    {
        CreateMeshData();
        CreateMesh();
    }

    [ContextMenu("Clear Mesh")]
    public void ClearMesh()
    {
        _vertPoints.Clear();
        _triangleIndex.Clear();
        _vertecies.Clear();
        this.GetComponent<MeshFilter>().mesh.Clear();
    }

    private void CreateMeshData()
    {
        _vertecies.Clear();
        _meshTriangles.Clear();

        for (int i = 0; i < _triangleIndex.Count; i++)
        {
            _vertecies.Add(transform.InverseTransformPoint( _vertPoints[_triangleIndex[i]]));
            _meshTriangles.Add(i);
        }
    }

    public void AddVertPoints(Vector3 vertPos)
    {
        if (!_points.Contains(vertPos))
        {
            _points.Add(vertPos);
        }

        _vertPoints.Add(vertPos);

    }

    private void CreateMesh()
    {
        Mesh mesh = this.GetComponent<MeshFilter>().mesh;
        mesh.Clear();
        mesh.vertices = _vertecies.ToArray();
        mesh.triangles = _meshTriangles.ToArray();
    }
}
