using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseFunctions : MonoBehaviour
{
    AudioManager audioManager;
    PInputManager pInputManager;
    [SerializeField] GameObject settings;
    [SerializeField] GameObject pause;
    [SerializeField] GameObject dead;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject darkenBackground;
    public static bool paused;
    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        pInputManager = FindObjectOfType<PInputManager>();
    }

    private void Update()
    {
        if (!paused&&pInputManager.pauseButton&& settings.activeSelf == false)
        {
            pInputManager.pauseButton = false;
            pInputManager.unpauseButton = false;
            Pause();
        }
        else if (settings.activeSelf == true && pInputManager.unpauseButton)
        {
            pInputManager.pauseButton = false;
            pInputManager.unpauseButton = false;
            Back();
        }
        else if (pause.activeSelf == true && pInputManager.unpauseButton)
        {
            pInputManager.pauseButton = false;
            pInputManager.unpauseButton = false;
            Resume();
        }
        else if (!paused)
        {
            Time.timeScale = 1;
        }
    }
    IEnumerator PauseBuffer()
    {
        Time.timeScale = 0.1f;
        darkenBackground.SetActive(true);
        UI.SetActive(false);
        pause.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        paused = true;
        yield return new WaitForSeconds(.05f);
        Time.timeScale = 0;
        
    }
    public void Pause()
    {
        audioManager.Play("Button Press");
        StartCoroutine(PauseBuffer());
    }
    // Update is called once per frame
    public void Resume()
    {
        darkenBackground.SetActive(false);
        UI.SetActive(true);
        audioManager.Play("Button Press");
        pause.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        paused = false;
        Time.timeScale = 1;
    }
    public void Settings()
    {
        audioManager.Play("Button Press");
        settings.SetActive(true);
        pause.SetActive(false);
    }
    public void Back()
    {
        audioManager.Play("Button Press");
        settings.SetActive(false);
        pause.SetActive(true);
    }
    public void Menu()
    {
        paused = false;
        darkenBackground.SetActive(false);
        audioManager.Play("Button Press");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Died()
    {
        Time.timeScale = 0;
        dead.SetActive(true);
        darkenBackground.SetActive(true);
        UI.SetActive(false);
        if (paused)
        {
            pause.SetActive(false);
        }
    }
    public void Retry()
    {
        audioManager.Play("Begin Game");
        dead.SetActive(false);
        darkenBackground.SetActive(false);
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Itch()
    {
        audioManager.Play("Button Press");
        Application.OpenURL("https://connroo.itch.io/");
    }
}
