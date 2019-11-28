using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
   public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale,  int octaves, float persistance, float lacumarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);

        if (scale<=0.0)
        {
            scale = 0.0001f;
        }

        Vector2[] octaveOffsets = new Vector2[octaves];

        for (var i=0;i<octaves;i++)
        {
            float offsetX = prng.Next(-10000, 10000)+offset.x;
            float offsetY = prng.Next(-10000, 10000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

        }

        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        for (var y=0;y<mapHeight;y++)
        {
            for (var x=0;x<mapWidth;x++)
            {
                float amplitude = 1;
                float frequence = 1;
                float noiseHeight = 0;

                for (var i = 0;i < octaves;i++) {
                    float sampleX = x / scale * frequence + octaveOffsets[i].x;
                    float sampleY = y / scale * frequence + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY)*2-1;
                    noiseHeight += perlinValue * amplitude;

                    amplitude *= persistance;
                    frequence *= lacumarity;
                }

                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight<minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }

                noiseMap[x, y] = noiseHeight;
            }
        }

        for (var y = 0; y < mapHeight; y++)
        {
            for (var x = 0; x < mapWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;
    }
}
