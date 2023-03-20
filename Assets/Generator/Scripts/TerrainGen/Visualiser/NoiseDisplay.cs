using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseDisplay : MonoBehaviour
{
    public Renderer renderer;
    public void GenTexture(float [,] noisemap)
    {
        int width = noisemap.GetLength(0);
        int height = noisemap.GetLength(1);

        Texture2D texture = new Texture2D(width, height);

        Color[] colourmap = new Color[width * height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                colourmap[y * width + x] = Color.Lerp(Color.black, Color.white, noisemap[x, y]);
            }
        }
        texture.SetPixels(colourmap);
        texture.Apply();
        renderer.sharedMaterial.mainTexture = texture;
        renderer.transform.localScale = new Vector3 (width, 1, height);
    }
}

