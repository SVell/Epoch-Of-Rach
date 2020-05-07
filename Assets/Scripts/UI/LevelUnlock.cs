using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{
    public Button[] buttons;

    private void Start()
    {
        int levelAt = PlayerPrefs.GetInt("levelAt",1);

        for (int i = 0; i < buttons.Length; i++)
        {
            if (i + 1 > levelAt)
                buttons[i].interactable = false;
        }
    }
}
