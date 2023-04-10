using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class TerrainGenerator : MonoBehaviour
{
    public NavMeshSurface navMesh;
    public NavMeshData data;
    public Vector3 position;
    public GameObject mapMesh;
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
    public Color[] colour;
    float[,] noiseSaved;
    bool generated = false;

    private void Start()
    {
        noiseGen = GetComponent<NoiseGenerator>();
        oceanFalloff = falloff.GenOcean(terrainWidth, terrainHeight);
        GenTerrain();
    }

    private void Update()
    {
        if (generated)
        {
            //CheckCell(noiseSaved, colour, areas, position);
        }
    }
    public void GenTerrain()
    {
        var rand = Random.Range(0, 100000);
        float[,] noise = NoiseGenerator.GenNoiseMap(noiseScale, terrainWidth, terrainHeight, octaves, persistance, lacrunarity, rand);

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

        Display display = FindObjectOfType<Display>();
        //display.Draw(TextureDisplay.ColourMap(colourMapping, terrainWidth, terrainHeight));
        //display.Draw(TextureDisplay.HeightMap(OceanFalloff.GenOcean(terrainHeight, terrainWidth)));
        display.MeshDraw(MeshGen.MeshGenerate(noise, smoothingCurve, heightScalar), TextureDisplay.ColourMap(colourMapping, terrainWidth, terrainHeight));
        mapMesh.AddComponent<MeshCollider>();
        //navMesh.UpdateNavMesh(data);
        PlaceTrees(noise, heightScalar, colourMapping, areas);
        colour = colourMapping;
        generated = true;
    }

    public void CheckCell(float[,] noise, Color[] colourMapping, MapAreas[] regions, Vector3 pos)
    {
        Vector3 cellPos = new Vector3(pos.x / noiseScale, pos.y, pos.z / noiseScale);
        Debug.Log(cellPos);
        /*
        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                for (int i = 0; i < areas.Length; i++)
                {
                    
                }
            }
        }*/
    }

    public void PlaceTrees(float[,] heightmap, float heightscalar, Color[] colourMapping, MapAreas[] regions)
    {
        int terrainWidth = heightmap.GetLength(0);
        int terrainHeight = heightmap.GetLength(1);
        for (int y = 0; y < terrainHeight; y++)
        {
            for (int x = 0; x < terrainWidth; x++)
            {
                float height = heightmap[x, y];
                if(colourMapping[y * terrainWidth + x] == regions[3].colour)
                {
                    if (terrainHeight > 0.5f)
                    {
                        GameObject placeTree = Instantiate(tree);
                        placeTree.transform.parent = treeParent.transform;
                        placeTree.transform.localPosition = new Vector3(x * noiseScale, -200, y * noiseScale);
                        placeTree.transform.localPosition = new Vector3(placeTree.transform.position.x + Random.RandomRange(-5, 5), placeTree.transform.position.y, placeTree.transform.position.z + Random.RandomRange(-5, 5));
                    }
                }
            }
        }
        treeParent.transform.position = new Vector3(-12650, 0, 12650);
        treeParent.transform.localScale = new Vector3(5, 5, 5);
        treeParent.transform.eulerAngles = new Vector3(
            treeParent.transform.eulerAngles.x,
            treeParent.transform.eulerAngles.y + 180,
            treeParent.transform.eulerAngles.z + 180);
    }
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
        mesh.RecalculateBounds();
        return mesh;
    }
}



[System.Serializable]
public struct MapAreas
{
    public string name; public float height; public Color colour;
}