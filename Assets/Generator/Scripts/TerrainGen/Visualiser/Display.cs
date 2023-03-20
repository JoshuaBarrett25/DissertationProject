using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter Filter;
    public MeshRenderer Renderer;

    public void Draw(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void MeshDraw(Data mesh, Texture2D texture)
    {
        Filter.sharedMesh = mesh.MapCreate();
        Renderer.sharedMaterial.mainTexture = texture;
    }
}
