using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour
{
    public int mouseSens;
    public Vector2 resolution;
    public int renderDistance;

    private void Update()
    {
        DontDestroyOnLoad(this);
    }
}
