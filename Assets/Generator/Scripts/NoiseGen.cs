using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGen : MonoBehaviour
{
    public static Vector2 GenNoiseMaps(float mapScale, Vector2 dimensions)
    {
        Vector2 mapNoise;
        mapNoise.x = dimensions.x;
        mapNoise.y = dimensions.y;

        //Robustness check
        if (mapScale <= 0.0f)
        {
            mapScale = 0.001f;
        }

        for (int y = 0; y < dimensions.y; y++)
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                float scaledX = x / mapScale;
                float scaledY = y / mapScale;
                //Noise generated using scaled vectors
                float noiseFactor = Mathf.PerlinNoise(scaledX, scaledY);
                mapNoise.x = noiseFactor;
                mapNoise.y = noiseFactor;
            }
        }
        return mapNoise;
    }
}
