using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NoiseGenerator : MonoBehaviour
{
    public static float[,] GenNoiseMap(float terrainScale, int terrainWidth, int terrainHeight, int oct, float per, float lac, int seed)
    {
        float[,] noiseMap = new float[terrainWidth, terrainHeight];
        float maxHeight = float.MinValue;
        float minHeight = float.MaxValue;

        System.Random rnd = new System.Random(seed);

        Vector2[] offsetting = new Vector2[oct];
        for (int i = 0; i < oct; i++)
        {
            float offsetX = rnd.Next(-200000, 2000000);
            float offsetY = rnd.Next(-200000, 2000000);
            offsetting[i] = new Vector2(offsetX, offsetY);
        }

        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                float amp = 1.0f;
                float freq = 1.0f;
                float height = 0;
                for (int o = 0; o < oct; o++)
                {
                    float tx = x / terrainScale * freq + offsetting[o].x;
                    float ty = y / terrainScale * freq + offsetting[o].y;


                    float perlin = Mathf.PerlinNoise(tx, ty) * 2 - 1;
                    height += perlin * amp;

                    amp *= per;
                    freq *= lac;
                }

                if (height > maxHeight)
                {
                    maxHeight = height;
                }

                else if (height < minHeight)
                {
                    minHeight = height;
                }

                noiseMap[x, y] = height;
            }
        }

        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                noiseMap[x, y] = Mathf.InverseLerp(minHeight, maxHeight, noiseMap[x, y]);
            }
        }

        return noiseMap;

    }
}
