using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    private int nextScene;

    public bool playStart = true;
    
    
    public GameObject gameComplete;
    public Animator anim;
    
    public static int numberOfLevels = 8;
    
    private void Start()
    {
        nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        anim.SetInteger("Open",1);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") + other.gameObject.GetComponent<playerController>().getCoins());
            // TODO: Check 6 or 7
            StartCoroutine(Load());
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    public IEnumerator Load()
    {
        anim.SetInteger("Open",2);
        yield return new WaitForSeconds(0.45f);


        if (nextScene > PlayerPrefs.GetInt("levelAt"))
        {
            PlayerPrefs.SetInt("levelAt",nextScene);
        }
        if (SceneManager.GetActiveScene().buildIndex+1 > numberOfLevels)
        {
            gameComplete.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(nextScene);
        }
        
    }
    
}
