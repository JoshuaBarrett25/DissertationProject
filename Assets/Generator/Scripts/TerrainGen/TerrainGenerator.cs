using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TerrainGenerator : MonoBehaviour
{
    public GameObject treeParent;
    public GameObject tree;
    public int terrainWidth;
    public int terrainHeight;
    public float heightScalar;
    public MapAreas[] areas;

    [Header("Noise Variables")]
    public bool updateNoiseInRealTime;
    [Range(1,20)]public int octaves;
    [Range(-0.5f, 1f)]public float persistance;
    [Range(0.0f, 2.5f)]public float lacrunarity;
    [Range(0.001f, 20f)] public float noiseScale;
    public int seed = 0;

    public AnimationCurve smoothingCurve;

    NoiseGenerator noiseGen;
    public OceanFalloff falloff;
    float[,] oceanFalloff;


    private void Start()
    {
        noiseGen = GetComponent<NoiseGenerator>();
        oceanFalloff = falloff.GenOcean(terrainWidth, terrainHeight);
    }




    public void GenTerrain()
    {

        float[,] noise = NoiseGenerator.GenNoiseMap(noiseScale, terrainWidth, terrainHeight, octaves, persistance, lacrunarity, seed);

        Color[] colourMapping = new Color[terrainWidth * terrainHeight];
        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                
                noise[x, y] = noise[x, y] - (oceanFalloff[x, y] - 0.41f);
                float height = noise[x, y];

                for (int i = 0; i < areas.Length; i++)
                {
                    if (height <= areas[i].height)
                    {
                        colourMapping[y * terrainWidth + x] = areas[i].colour;
                        break;
                    }
                }
            }
        }

        //PlaceTrees(noise, colourMapping, areas);
        Display display = FindObjectOfType<Display>();
        //display.Draw(TextureDisplay.ColourMap(colourMapping, terrainWidth, terrainHeight));
        //display.Draw(TextureDisplay.HeightMap(OceanFalloff.GenOcean(terrainHeight, terrainWidth)));
        display.MeshDraw(MeshGen.MeshGenerate(noise, smoothingCurve, heightScalar), TextureDisplay.ColourMap(colourMapping, terrainWidth, terrainHeight));
    }


    /*
    public void PlaceTrees(float[,] heightmap, Color[] colourMapping, MapAreas[] regions)
    {
        int terrainWidth = heightmap.GetLength(0);
        int terrainHeight = heightmap.GetLength(1);
        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                if(colourMapping[y * terrainWidth + x] == regions[5].colour)
                {
                    if (terrainHeight > 0.5f)
                    {
                        GameObject placeTree = Instantiate(tree);
                        placeTree.transform.parent = this.transform;
                        placeTree.transform.localPosition = new Vector3(x * noiseScale, 0, y * noiseScale);
                    }
                }
            }
        }
        this.transform.position = new Vector3(-2400, 0, -2400);
        treeParent.transform.position.Scale(new Vector3(20, 20, 20));
    }
    */
}



public static class MeshGen
{
    public static Data MeshGenerate(float [,] heightMap, AnimationCurve smoothcurve, float scale)
    {
        int meshWidth = heightMap.GetLength(0);
        int meshHeight = heightMap.GetLength(1);
        Data mData = new Data(meshWidth, meshHeight);
        int vertInd = 0;

        float lX, lY;
        lX = (meshWidth - 1) / -2;
        lY = (meshHeight - 1) / 2f;

        for (int y = 0; y < meshWidth; y++)
        {
            for (int x = 0; x < meshHeight; x++)
            {
                mData.vert[vertInd] = new Vector3(lX + x, smoothcurve.Evaluate(heightMap[x, y]) * scale, lY - y);
                mData.uvs[vertInd] = new Vector2(x / (float)meshWidth, y / (float)meshHeight);

                if (x < meshWidth - 1 && y < meshHeight - 1)
                {
                    mData.Triangles(vertInd, vertInd + meshWidth + 1, vertInd + meshWidth);
                    mData.Triangles(vertInd + meshWidth + 1, vertInd, vertInd + 1);
                }

                vertInd++;
            }
        }
        return mData;
    }

}



public class Data
{
    public Vector3[] vert;
    public int[] tri;
    public Vector2[] uvs;

    int triInd;

    public Data(int meshWidth, int meshHeight)
    {
        vert = new Vector3[meshWidth * meshHeight];
        tri = new int[(meshWidth - 1) * (meshHeight - 1)*6];
        uvs = new Vector2[meshWidth * meshHeight];
    }

    public void Triangles(int a, int b, int c)
    {
        tri[triInd] = a;
        tri[triInd + 1] = b;
        tri[triInd + 2] = c;
        triInd += 3;
    }

    public Mesh MapCreate()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vert;
        mesh.triangles = tri;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return mesh;
    }
}



[CustomEditor(typeof(TerrainGenerator))]
[CanEditMultipleObjects]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        TerrainGenerator genMap = (TerrainGenerator)target;
        DrawDefaultInspector();

        if (genMap.updateNoiseInRealTime)
        {
            genMap.GenTerrain();
        }

        if (GUILayout.Button("Generate Map"))
        {
            genMap.GenTerrain();
        }
   }
}



[System.Serializable]
public struct MapAreas
{
    public string name; public float height; public Color colour;
}