using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGen : MonoBehaviour
{
    public Vector2 dimensions; public float mapNoiseScale;
    Renderer noiseRenderer;

    //Generate TerrainObject. Will be the base for terrain generation
    public void GenPlane()
    {
        GameObject mapPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mapPlane.name = "MapTerrainPlane";
        mapPlane.tag = "Terrain";
        mapPlane.transform.position = new Vector3(0, 0, 0);
        mapPlane.GetComponent<MeshRenderer>().material = Resources.Load<Material>("Materials/Noise");
        Material mat = Resources.Load<Material>("Materials/Noise");
        noiseRenderer = mapPlane.GetComponent<Renderer>();
        GenMap();
    }

    public void Export()
    {
        GameObject[] terrainGameObjects;
        terrainGameObjects = GameObject.FindGameObjectsWithTag("Terrain");

        if (terrainGameObjects != null)
        {
            //Create new asset folder for the exported meshes
            string path = "" + System.DateTime.Now;
            AssetDatabase.CreateFolder("Assets/ExportedAssets", path);
            string[] folder = AssetDatabase.GetSubFolders("Assets/ExportedAssets");
            string localPath = folder[folder.Length-1] + "/" + gameObject.name + ".prefab";

            for (int itr = 0; itr < terrainGameObjects.Length; itr++)
            {
                PrefabUtility.SaveAsPrefabAsset(terrainGameObjects[itr], localPath);
                Debug.Log("Model has been successfully exported: " + terrainGameObjects[itr].name);
            }
        }
    }

    public void GenMap()
    {
        float[,] generatedMapNoise = NoiseGen.GenNoiseMaps(mapNoiseScale, (int)dimensions.x, (int)dimensions.y);
        NoiseDisplay noiseDisplay = new NoiseDisplay();
        noiseDisplay.RenderNoise(generatedMapNoise, noiseRenderer);
    }
}

[CustomEditor (typeof (MapGen)), CanEditMultipleObjects]
public class MapEditor : Editor
{
    public override void OnInspectorGUI()
    {
        MapGen mapGen;
        mapGen = (MapGen)target;

        DrawDefaultInspector();

        if (GUILayout.Button("Generate Plane"))
        {
            mapGen.GenPlane();
        }

        if (GUILayout.Button("Export Model"))
        {
            mapGen.Export();
        }
    }
}

public class NoiseDisplay : MonoBehaviour
{
    public void RenderNoise(float[,] noise, Renderer noiseRenderer)
    {
        Vector2 dimensions;
        dimensions.x = noise.GetLength(0);
        dimensions.y = noise.GetLength(1);

        Texture2D texture = new Texture2D((int)dimensions.x, (int)dimensions.y);

        Color[] colour = new Color[(int)dimensions.x * (int)dimensions.y];
        for (int y = 0; y < dimensions.y; y++)
        {
            for (int x = 0; x < dimensions.x; x++)
            {
                colour[y * (int)dimensions.x + x] = Color.Lerp(Color.black, Color.white, noise[x, y]);
            }
        }
        texture.SetPixels(colour);
        texture.Apply();
        noiseRenderer.sharedMaterial.mainTexture = texture;
        noiseRenderer.transform.localScale = new Vector3(dimensions.x, 1, dimensions.y);
    }
}