using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapDisplay : MonoBehaviour
{
    [SerializeField] private Renderer _textureRenderer;
    [SerializeField] private MeshFilter _meshFilter;
    [SerializeField] private MeshRenderer _meshRenderer;
    [SerializeField] private MeshCollider _terrainCollider;

    public void DrawTexture(Texture2D texture)
    {
        _textureRenderer.material.mainTexture = texture;
        _textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        _meshFilter.sharedMesh = meshData.CreateMesh();
        _meshFilter.sharedMesh.name = "shader";
        _meshRenderer.sharedMaterial.mainTexture = texture;
        _terrainCollider.sharedMesh = _meshFilter.sharedMesh;
    }
}
