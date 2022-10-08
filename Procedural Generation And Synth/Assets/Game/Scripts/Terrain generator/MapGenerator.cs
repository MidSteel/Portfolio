using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TerrainRenderType { Noise, Color, Mesh}

public class MapGenerator : MonoBehaviour
{
    private const int _MapChunkSize = 241;

    [SerializeField][Range(0, 6)] private int _levelOfDetail = 1;
    [SerializeField] private TerrainRenderType _renderType = TerrainRenderType.Noise;
    [SerializeField] private Biome _biome;
    [SerializeField][Range(1f, 100f)]private float _scale;
    [SerializeField][Range(1, 10)] private int _octaves;
    [SerializeField][Range(0.001f, 1f)] private float _persistance;
    [SerializeField][Range(1f, 10f)] private float _lacunarity;
    [SerializeField] private int _seed;
    [SerializeField] private Vector2 _offset;
    [SerializeField] private AnimationCurve _heightCurve;
    [SerializeField][Range(0f, 100f)] private float _heightMultiplaier;

    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(_MapChunkSize, _MapChunkSize, _seed, _scale, _octaves, _persistance, _lacunarity, _offset);
        MapDisplay mapDisplay = this.GetComponent<MapDisplay>();
        Color[] colorMap = new Color[_MapChunkSize* _MapChunkSize];
        float halfChunkSize = _MapChunkSize / 2f;

        for (int y = 0; y < _MapChunkSize; y++)
        {
            for (int x = 0; x < _MapChunkSize; x++)
            {
                if (_biome._biomes.Length <= 1)
                {

                    foreach (TerrainType tType in _biome._biomes)
                    {
                        foreach (TerrainTypeData typeData in tType.terrainTypes)
                        {
                            if (typeData.typeHeight >= noiseMap[x, y]) { colorMap[y * _MapChunkSize + x] = typeData.typeColor; break; }
                        }
                    }
                }
                else
                {
                    Color color = Color.clear;
                    //foreach (TerrainType tType in _biome._biomes)
                    //{
                    //    //foreach (TerrainTypeData typeData in tType.terrainTypes)
                    //    //{
                    //    //    if (typeData.typeHeight >= noiseMap[x, y]) { colorMap[y * _MapChunkSize + x] = typeData.typeColor; break; }
                    //    //}

                    //    //for (int i = 0; i < tType.terrainTypes.Length; i++)
                    //    //{
                    //    //    if (tType.terrainTypes[i].typeHeight >= noiseMap[x, y]) { if (color == Color.clear) { color = tType.terrainTypes[i].typeColor; break; } else { color = Color.Lerp(color, tType.terrainTypes[i].typeColor, noiseMap[x, y]); break; } }
                    //    //}

                    //    if (x <= halfChunkSize)
                    //    {
                    //        for (int i = 0; i < tType.terrainTypes.Length; i++)
                    //        {
                    //            if (tType.terrainTypes[i].typeHeight >= noiseMap[x, y]) { colorMap[_MapChunkSize * y + x] = }
                    //        }
                    //    }

                    //    colorMap[ y * _MapChunkSize + x] = color;
                    //}

                    //Debug.Log("BIOMEs : " + _biome._biomes.Length, this);
                    TerrainType tTYpe = _biome._biomes[0];
                    if (x <= (_MapChunkSize * Mathf.Sin(noiseMap[x, y])))
                    {
                        tTYpe = _biome._biomes[1];
                    }

                    for (int i = 0; i < tTYpe.terrainTypes.Length; i++)
                    {
                        if (tTYpe.terrainTypes[i].typeHeight >= noiseMap[x, y]) { color = tTYpe.terrainTypes[i].typeColor; break; }
                    }

                    colorMap[y * _MapChunkSize + x] = color;
                }
                
                //foreach (TerrainType tType in _biome._biomes)
                //{
                //    foreach (TerrainTypeData typeData in tType.terrainTypes)
                //    {
                //        if (typeData.typeHeight >= noiseMap[x, y]) { colorMap[y * _MapChunkSize + x] = typeData.typeColor; break; }
                //    }
                //}
            }
        }

        if (_renderType == TerrainRenderType.Noise) { mapDisplay.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); }
        else if (_renderType == TerrainRenderType.Color) { mapDisplay.DrawTexture(TextureGenerator.TextureFromColorMap(colorMap, _MapChunkSize, _MapChunkSize)); }
        else if (_renderType == TerrainRenderType.Mesh) { mapDisplay.DrawMesh(MeshGenerator.GenrateTerrainMesh(noiseMap, _heightCurve, _heightMultiplaier, _levelOfDetail), TextureGenerator.TextureFromColorMap(colorMap, _MapChunkSize, _MapChunkSize)); }
    }

    public void OnValidate()
    {
        GenerateMap();
    }
}
