using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TextureDisplay
{
    public static Texture2D ColourMap(Color[] cMap, int width, int height)
    {
        Texture2D text = new Texture2D(width, height);
        text.wrapMode = TextureWrapMode.Clamp;
        text.filterMode = FilterMode.Point;
        text.SetPixels(cMap);
        text.Apply();
        return text;
    }

    public static Texture2D HeightMap(float[,] hMap)
    {
        int terrainWidth = hMap.GetLength(0);
        int terrainHeight = hMap.GetLength(1);
        Color[] colourMapping = new Color[terrainWidth * terrainHeight];

        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                colourMapping[y * terrainWidth + x] = Color.Lerp(Color.black, Color.white, hMap[x, y]);
                break;
            }
        }
        return ColourMap(colourMapping, terrainWidth, terrainHeight);
    }
}
