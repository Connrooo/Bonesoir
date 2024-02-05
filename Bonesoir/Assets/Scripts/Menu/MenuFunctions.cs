using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject settings;
    AudioManager audioManager;
    [SerializeField] Animator background;

    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Enter()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        background.SetTrigger("startGame");
        audioManager.Play("Begin Game");
        menu.SetActive(false);
        StartCoroutine(Begin());
    }
    IEnumerator Begin()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
    public void Settings()
    {
        audioManager.Play("Button Press");
        menu.SetActive(false);
        settings.SetActive(true);
    }

    public void Quit()
    {
        audioManager.Play("Button Press");
        Application.Quit();
    }

    public void Back()
    {
        audioManager.Play("Button Press");
        settings.SetActive(false);
        menu.SetActive(true);
    }

    public void Itch()
    {
        audioManager.Play("Button Press");
        Application.OpenURL("https://connroo.itch.io/");
    }
}
