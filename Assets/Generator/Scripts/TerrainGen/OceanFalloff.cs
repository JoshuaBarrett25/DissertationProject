using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanFalloff : MonoBehaviour
{
    public float[,] GenOcean(int mapSizeX, int mapSizeY)
    {
        float[,] terrain = new float[mapSizeX, mapSizeY];

        for (int q = 0; q < mapSizeX; q++)
        {
            for (int w = 0; w < mapSizeY; w++)
            {
                float x = q / (float) mapSizeX * 2 - 1;
                float y = w / (float) mapSizeY * 2 - 1;

                float value = Mathf.Max(Mathf.Abs(x), Mathf.Abs(y));
                terrain[q, w] = value;
            }
        }
        return terrain;
    }
}
