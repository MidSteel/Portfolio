using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int width, int height, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[width, height];
        System.Random prng = new System.Random(seed);
        Vector2[] offsets = new Vector2[octaves];
        float halfWidth = width / 2f;
        float halfHeight = height / 2f;

        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            offsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0) { scale = 0.0001f; }
        float maxNoiseHeight = float.MinValue;
        float minNoisHeight = float.MaxValue;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float noiseHeight = 0f;
                float frequency = 1f;
                float amplitude = 1f;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = ((x - halfWidth ) / scale + offsets[i].x) * frequency;
                    float sampleY = ((y - halfHeight ) / scale + offsets[i].y) * frequency;
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2f - 1f;
                    noiseHeight += perlinValue * amplitude;
                    amplitude *= persistance;
                    frequency *= lacunarity;
                }
                if (noiseHeight > maxNoiseHeight) { maxNoiseHeight = noiseHeight; } else if (noiseHeight < minNoisHeight) { minNoisHeight = noiseHeight; }
                noiseMap[x, y] = noiseHeight;
            }
        }

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoisHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
