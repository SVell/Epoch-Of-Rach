using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public Text coins;
    
    public Text text1;
    public Text text2;
    
    public Text text3;
    public Text text4;
    
    public Text text5;
    public Text text6;
    
    
    public Button[] buttons;

    private void Start()
    {
        //PlayerPrefs.SetInt("coins",250);
        if (buttons.Length != 0)
        {
            if (PlayerPrefs.GetInt("button0") == 1)
            {
                buttons[0].interactable = false;
                text1.gameObject.SetActive(false);
                text2.gameObject.SetActive(true);
            }
            if (PlayerPrefs.GetInt("button1") == 1)
            {
                buttons[1].interactable = false;
                text3.gameObject.SetActive(false);
                text4.gameObject.SetActive(true);
            }
            if (PlayerPrefs.GetInt("button2") == 1)
            {
                buttons[2].interactable = false;
                text5.gameObject.SetActive(false);
                text6.gameObject.SetActive(true);
            }
        }
    }

    private void Update()
    {
        coins.text = "x" + PlayerPrefs.GetInt("coins");
    }

    public void IncreaseHP()
    {
        if (PlayerPrefs.GetInt("coins") >= 100)
        {
            PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") - 100);
            PlayerPrefs.SetInt("hp",20);
            text1.gameObject.SetActive(false);
            text2.gameObject.SetActive(true);
            PlayerPrefs.SetInt("button0", 1);
        }
    }

    public void IncreaseDamage()
    {
        if (PlayerPrefs.GetInt("coins") >= 100)
        {
            PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") - 100);
            PlayerPrefs.SetInt("damage",20);
            text3.gameObject.SetActive(false);
            text4.gameObject.SetActive(true);
            PlayerPrefs.SetInt("button1", 1);
        }
        
    }

    public void StartingKnife()
    {
        if (PlayerPrefs.GetInt("coins") >= 100)
        {
            PlayerPrefs.SetInt("coins",PlayerPrefs.GetInt("coins") - 100);
            PlayerPrefs.SetInt("knifeCount",1);
            text5.gameObject.SetActive(false);
            text6.gameObject.SetActive(true);
            PlayerPrefs.SetInt("button2", 1);
        }
    }
}
