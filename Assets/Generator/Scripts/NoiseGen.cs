using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGen : MonoBehaviour
{
    //Generates our noise map using the unity math function for perlin noise.
    //Takes in parameters set by the user to scale the noise and place it on our terrain yielding different generations
    public static float[,] GenNoiseMaps(float mapScale, int width, int height)
    {
        float[,] mapNoise = new float[width,height];

        //Robustness check to make sure the scale is at least a value above 0 otherwise no noisemap will be present
        if (mapScale <= 0.0f)
        {
            mapScale = 0.001f;
        }

        //Scales our noise map along the dimensions the user has changed
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float scaledX = x / mapScale;
                float scaledY = y / mapScale;
                //Noise generated using scaled vectors
                float noiseFactor = Mathf.PerlinNoise(scaledX, scaledY);
                mapNoise[x, y] = noiseFactor;
            }
        }
        return mapNoise;
    }
}
