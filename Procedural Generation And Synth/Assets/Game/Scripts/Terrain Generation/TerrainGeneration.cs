using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TerrainGeneration.Scriptables;

namespace TerrainGeneration
{
    public class TerrainGeneration : MonoBehaviour
    {
        [SerializeField] private TerrainBiomeData[] _terrainBiomeData;
        [SerializeField] private Terrain _terrain;
        [SerializeField] private float _scale;
        [SerializeField] [Range(1, 10)] private int _octaves;
        [SerializeField] [Range(0.001f, 1f)] private float _persistance;
        [SerializeField] [Range(1f, 10f)] private float _lacunarity;
        [SerializeField] private int _seed;
        [SerializeField] private Vector2 _offset;
        [SerializeField] private AnimationCurve _heightMultiplier;

        public void GenerateTerrain()
        {
            TerrainData terrainData = _terrain.terrainData;
            float[,] noiseMap = TerrainNoise.GenerateTerrainNoise(terrainData.alphamapWidth + 1, terrainData.alphamapHeight + 1, _seed, _scale, _octaves, _persistance, _lacunarity, _offset, _heightMultiplier);
            terrainData.SetHeights(0, 0, noiseMap);
            float[,,] textureMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
            AddTextureLayers();
            ApplyTexture();
        }

        public void AddTextureLayers()
        {
            if (_terrainBiomeData.Length <= 0) { return; }

            TerrainData terrainData = _terrain.terrainData;
            terrainData.terrainLayers = null;
            List<TerrainLayer> textureLayers = new List<TerrainLayer>();

            foreach (BiomeData biomeData in _terrainBiomeData[0].BiomeData)
            {
                TerrainLayer layer = new TerrainLayer();
                layer.name = biomeData.name;
                layer.diffuseTexture = biomeData.texture;
                textureLayers.Add(layer);
            }

            terrainData.terrainLayers = textureLayers.ToArray();
        }

        public void ApplyTexture()
        {
            TerrainData terrainData = _terrain.terrainData;
            float[,,] textureMap = new float[terrainData.alphamapWidth, terrainData.alphamapHeight, terrainData.alphamapLayers];
            int height = terrainData.alphamapHeight;
            int width = terrainData.alphamapWidth;
            float[,] heights = terrainData.GetHeights(0, 0, width, height);
            float[] textureMapValues = new float[_terrainBiomeData[0].BiomeData.Length];

            for (int y = 0; y < height; y++)
            {
                for ( int x = 0; x < width; x++)
                {
                    float terrainHeight = heights[x, y];
                    //int textureMapLength = textureMapValues.Length;

                    int biomeDataLength = _terrainBiomeData[0].BiomeData.Length;

                    for (int i = 0; i < biomeDataLength; i++)
                    {
                        float prevHeight = 0f;
                        if (i > 0) { prevHeight = _terrainBiomeData[0].BiomeData[i - 1].height; }

                        //if (i == 0) { textureMapValues[i] = 1f; }
                        if (terrainHeight <= _terrainBiomeData[0].BiomeData[i].height && terrainHeight >= prevHeight)
                        {
                            textureMapValues[i] = 1f;
                        }
                        else if (terrainHeight >= _terrainBiomeData[0].BiomeData[i].height)
                        {
                            textureMapValues[i] = 1f;
                        }
                        else
                        {
                            textureMapValues[i] = -1f;
                        }
                    }

                    for (int i = 0; i < biomeDataLength; i++)
                    {
                        textureMap[x, y, i] = textureMapValues[i];
                    }
                }
            }

            terrainData.SetAlphamaps(0, 0, textureMap);
        }

        public void OnValidate()
        {
            GenerateTerrain();
        }
    }
}
