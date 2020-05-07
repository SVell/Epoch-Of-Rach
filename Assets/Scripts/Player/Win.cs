using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private int nextScene;

    public static int numberOfLevels = 6;
    
    private void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") + other.gameObject.GetComponent<playerController>().getCoins());
            // TODO: Check 6 or 7
            if (SceneManager.GetActiveScene().buildIndex == numberOfLevels)
            {
                SceneManager.LoadScene("MainMenu");
            }
            
            SceneManager.LoadScene(nextScene);

            if (nextScene > PlayerPrefs.GetInt("levelAt"))
            {
                PlayerPrefs.SetInt("levelAt",nextScene);
            }
        }
    }
}
