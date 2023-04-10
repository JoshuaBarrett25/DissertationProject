using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BuildingGeneration : MonoBehaviour
{
    public GameObject placedFlag;
    public GameObject mapMesh;
    public GameObject villager;
    public Transform empty;
    //public FlagPlacement flag;
    public bool canBuild;
    public ResourceManager resources;
    public GameObject house;
    //public GameObject[] houses;
    //public int[] cost;
    //public int[] profit;

    public bool CheckRequirements(int buildCost)
    {
        if (buildCost > resources.wood)
        {
            return false;
        }
        else
        {
            resources.wood -= buildCost;
            return true;
        }
    }

    public void BuildHouse()
    {
        if (CheckRequirements(100))
        {
            Vector3 pos = new Vector3(placedFlag.transform.position.x, placedFlag.transform.position.y, placedFlag.transform.position.z);
            empty.position = pos;
            var instance = Instantiate(house, empty);
            instance.transform.parent = null;
            PlaceVillager();
        }
    }

    private void PlaceVillager()
    {
        Vector3 pos = new Vector3(placedFlag.transform.position.x + 90, placedFlag.transform.position.y + 50, placedFlag.transform.position.z);
        empty.position = pos;
        var character = Instantiate(villager, empty);
        character.AddComponent<NavMeshAgent>();
        character.transform.parent = null;
    }

    private void Update()
    {
        if (canBuild)
        {
            BuildHouse();
        }
    }
}
