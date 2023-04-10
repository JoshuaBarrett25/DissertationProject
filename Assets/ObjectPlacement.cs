using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacement : MonoBehaviour
{
    public GameObject mesh;
    private bool isMoving = true;
    public float speed = 100f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Terrain"))
        {
            Debug.Log("Hit");
            isMoving = false;
            Destroy(this);
        }
    }

    private void Update()
    {
        if (isMoving)
        {
            mesh.transform.position = new Vector3(mesh.transform.position.x, mesh.transform.position.y - speed, mesh.transform.position.z);
        }
    }
}
