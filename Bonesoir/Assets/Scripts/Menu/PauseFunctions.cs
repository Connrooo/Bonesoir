using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
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
    [SerializeField] GameObject survived;
    [SerializeField] GameObject UI;
    [SerializeField] GameObject items;
    [SerializeField] GameObject darkenBackground;
    public bool dontPause;
    public static bool paused;
    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = FindObjectOfType<AudioManager>();
        pInputManager = FindObjectOfType<PInputManager>();
        darkenBackground.SetActive(false);
    }

    private void Update()
    {
        if (!paused&&pInputManager.pauseButton&& settings.activeSelf == false&&!dontPause)
        {
            pInputManager.pauseButton = false;
            pInputManager.unpauseButton = false;
            Pause();
        }
        else if (settings.activeSelf == true && pInputManager.unpauseButton && !dontPause)
        {
            pInputManager.pauseButton = false;
            pInputManager.unpauseButton = false;
            Back();
        }
        else if (pause.activeSelf == true && pInputManager.unpauseButton && !dontPause)
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
        items.SetActive(false);
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
        items.SetActive(true);
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
        audioManager.Stop("Low Pitch Noise");
        audioManager.Stop("High Pitch Noise");
        NewEnemyInteract.grabbed = false;
        paused = false;
        darkenBackground.SetActive(false);
        audioManager.Play("Button Press");
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void Died()
    {
        audioManager.Play("High Pitch Noise");
        Time.timeScale = 0;
        dead.SetActive(true);
        darkenBackground.SetActive(true);
        UI.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        paused = true;
        items.SetActive(false);
        Time.timeScale = 0;
    }

    public void Survived()
    {
        audioManager.Play("Low Pitch Noise");
        Time.timeScale = 0;
        survived.SetActive(true);
        darkenBackground.SetActive(true);
        UI.SetActive(false);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
        items.SetActive(false);
        paused = true;
        Time.timeScale = 0;
    }

    public void Retry()
    {
        audioManager.Stop("Low Pitch Noise");
        audioManager.Stop("High Pitch Noise");
        NewEnemyInteract.grabbed = false;
        audioManager.Play("Begin Game");
        dead.SetActive(false);
        Time.timeScale = 1;
        items.SetActive(true);
        paused = false;
        SceneManager.LoadScene(0);
        SceneManager.LoadScene(1);
    }
    public void Itch()
    {
        audioManager.Play("Button Press");
        Application.OpenURL("https://connroo.itch.io/");
    }
}
