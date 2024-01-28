using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject settings;

    public void Enter()
    {
        SceneManager.LoadScene(1);
    }
    public void Settings()
    {
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Back()
    {
        settings.SetActive(false);
        menu.SetActive(true);
    }

    public void Itch()
    {
        Application.OpenURL("https://connroo.itch.io/");
    }
}
