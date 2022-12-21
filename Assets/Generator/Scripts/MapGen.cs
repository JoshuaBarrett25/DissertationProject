using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapGen : MonoBehaviour
{
    public Vector2 mapDimensions; public float mapNoiseScale;

    public void GenPlane()
    {
        GameObject mapPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mapPlane.name = "MapTerrainPlane";
        mapPlane.tag = "Terrain";
        mapPlane.transform.position = new Vector3(0, 0, 0);
    }

    public void Export()
    {
        GameObject[] terrainGameObjects;
        terrainGameObjects = GameObject.FindGameObjectsWithTag("Terrain");
        Debug.Log(terrainGameObjects[0]);

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
            }
        }
    }

    public void GenMap()
    {
        Vector2 generatedMapNoise = NoiseGen.GenNoiseMaps(mapNoiseScale, mapDimensions);
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
