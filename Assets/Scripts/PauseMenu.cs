using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    static bool canBePaused = true;

    public GameObject pauseUi;
    public Animator pauseAnim;
    
    public GameObject dieUI;
    public Animator dieAnim;

    public AudioSource chain;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && canBePaused && pauseAnim!= null && pauseUi != null)
        {
            pauseAnim.SetInteger("Pause",2);
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        if (chain != null)
        {
            chain.Play();
        }
        
        pauseUi.SetActive(false);
        pauseAnim.SetInteger("Pause",1);
        gameIsPaused = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        if (chain != null)
        {
            chain.Play();
        }
        pauseUi.SetActive(true);
        pauseAnim.SetInteger("Pause",0);
        gameIsPaused = true;
        Time.timeScale = 0f;
    }

    public void LoadMenu()
    {
        canBePaused = true;
        Time.timeScale = 1f;
        gameIsPaused = false;
        // TODO: Load MainMenu
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    
    
    public void RestartLevel()
    {
        gameIsPaused = false;
        Time.timeScale = 1f;
        Scene scene = SceneManager.GetActiveScene();
        canBePaused = true;
        SceneManager.LoadScene(scene.name);
    }

    public void PlayeGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void Die()
    {
        if (chain != null)
        {
            chain.Play();
        }
        canBePaused = false;
        dieAnim.SetInteger("Die",1);
        dieUI.SetActive(true);
    }
}
