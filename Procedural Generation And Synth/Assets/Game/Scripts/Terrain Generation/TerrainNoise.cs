using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TerrainGeneration    
{
    public static class TerrainNoise
    {
        public static float[,] GenerateTerrainNoise(int width, int height, int seed, float scale, int octaves, float persistence, float lacunarity, Vector2 offset, AnimationCurve heightCurve)
        {
            float[,] noiseMap = new float[width, height];
            Vector2[] offsets = new Vector2[octaves];
            scale = scale == 0 ? 0.001f : scale;
            System.Random random = new System.Random(seed);

            for (int i = 0; i < octaves; i++)
            {
                float offsetX = random.Next(-100000, 100000) + offset.x;
                float offsetY = random.Next(-100000, 100000) + offset.y;
                offsets[i] = new Vector2(offsetX, offsetY);
            }

            float minNoiseHeight = float.MaxValue;
            float maxNoiseHeight = float.MinValue;
            float halfWidth = width / 2f;
            float halfHeight = height / 2f;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    float noiseHeight = 0f;
                    float frequency = 1f;
                    float amplitude = 1f;

                    for (int i = 0; i < octaves; i++)
                    {
                        float valueX = ((x - halfWidth) / scale + offsets[i].x) * frequency;
                        float valueY = ((y - halfHeight) / scale + offsets[i].y) * frequency;
                        float perLinValue = Mathf.PerlinNoise(valueX, valueY) * 2f - 1f;
                        noiseHeight += heightCurve.Evaluate(perLinValue * amplitude);
                        amplitude *= persistence;
                        frequency *= lacunarity;
                    }
                    if (noiseHeight > maxNoiseHeight) { maxNoiseHeight = noiseHeight; } else if (noiseHeight < minNoiseHeight) { minNoiseHeight = noiseHeight; }
                    noiseMap[x, y] = noiseHeight /***/ /*heightCurve.Evaluate(noiseHeight)*/;
                }
            }

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
                }
            }

            return noiseMap;
        }
    }
}