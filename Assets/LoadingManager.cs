using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    public GameObject loadScreen;
    public float loadTimer = 10f;
    public float textTimer = 0.5f;
    public TextMeshProUGUI[] loadingScreenText;

    private void Loading()
    {
        if (loadTimer > 0)
        {
            loadTimer -= Time.deltaTime;
            TextChange();
        }

        else
        {
            loadScreen.SetActive(false);
        }
    }

    private void TextChange()
    {
        if (textTimer > 0)
        {
            textTimer -= Time.deltaTime;
        }

        else if (textTimer <= 0)
        {
            loadingScreenText[0].text = loadingScreenText[0].text + ".";
            textTimer = 1.5f;
        }
    }

    private void Update()
    {
        Loading();
    }
}
