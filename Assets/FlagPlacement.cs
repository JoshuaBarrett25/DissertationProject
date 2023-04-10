using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   

public class FlagPlacement : MonoBehaviour
{
    public TerrainGenerator gen; 
    public BuildingGeneration buildingGen;
    public GameObject[] crosshairs;
    public GameObject transFlag;
    public GameObject placedFlag;

    public Vector3 pos;
    public Camera camera;
    public LayerMask layer;
    public Texture2D texture;

    private MapAreas[] areas;
    private Color[] colours;
    
    private bool canPlace = true;


    private void Start()
    {
        areas = gen.areas;
        colours = gen.colour;
    }

    private void Cast()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, 10000f, layer))
        {
            if (hit.collider.gameObject.tag == "Terrain")
            {
                texture = (Texture2D)hit.collider.gameObject.GetComponent<Renderer>().material.mainTexture;
            }
            var value = texture.GetPixelBilinear(hit.textureCoord.x, hit.textureCoord.y);

            for (int i = 0; i < areas.Length; i++)
            {
                if ((value.r >= areas[i].colour.r - 0.02 && value.r <= areas[i].colour.r + 0.02)
                    || (value.b >= areas[i].colour.b - 0.02 && value.b <= areas[i].colour.b + 0.02)
                    || (value.g >= areas[i].colour.g - 0.02 && value.g <= areas[i].colour.g + 0.02))
                {
                    Debug.Log(areas[i].name);
                    if ((areas[i].name == "DeepWater") || (areas[i].name == "Sand"))
                    {
                        canPlace = false;
                        crosshairs[0].SetActive(false);
                        crosshairs[1].SetActive(true);
                    }
                    else
                    {
                        canPlace = true;
                        crosshairs[0].SetActive(true);
                        crosshairs[1].SetActive(false);
                    }
                }
            }

            transFlag.transform.position = hit.point;
            pos = transFlag.transform.position;

            if (Input.GetMouseButtonUp(0) && canPlace)
            {
                PlaceFlag();
                Destroy(this);
            }
        }
    }



    private void PlaceFlag()
    {
        placedFlag.transform.position = transFlag.transform.position;
        Destroy(transFlag);
        buildingGen.canBuild = true;
        //Destroy(placedFlag.GetComponent<TreeClearing>());
    }



    private void Update()
    {
        gen.position = pos;
        Cast();
    }
}
