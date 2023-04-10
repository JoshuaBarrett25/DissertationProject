using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuFunctionality : MonoBehaviour
{
    GameSettings gameSettings;
    public GameObject mainOptions;
    public GameObject launchOptions;
    public GameObject settingsOptions;

    public TMPro.TextMeshProUGUI mouseText;



    private void Start()
    {
        gameSettings = FindObjectOfType<GameSettings>();
        gameSettings.mouseSens = 10;
    }

    private void Update()
    {
        mouseText.text = gameSettings.mouseSens.ToString();
    }

    public void PlaySelected()
    {
        mainOptions.SetActive(false);
        //launchOptions.SetActive(true);
        SceneManager.LoadSceneAsync("TerrainGeneration");
    }

    public void SettingsSelected()
    {
        mainOptions.SetActive(false);
        settingsOptions.SetActive(true);
    }

    public void QuitSelected()
    {
        Application.Quit();
    }

    public void ReturnSelected()
    {
        settingsOptions.SetActive(false);
        launchOptions.SetActive(false);
        mainOptions.SetActive(true);
    }


    public void MouseSensIncrease()
    {
        gameSettings.mouseSens += 1;
        if (gameSettings.mouseSens >= 15)
        {
            gameSettings.mouseSens = 15;
        }
    }

    public void MouseSenseDecrease()
    {
        gameSettings.mouseSens -= 1;
        if (gameSettings.mouseSens <= 0)
        {
            gameSettings.mouseSens = 0;
        }
    }
}
