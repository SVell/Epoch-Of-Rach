using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box : MonoBehaviour
{
    private SpriteRenderer sr;
    private int hitNum = 0;
    
    public Sprite newState;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void TakeDamage()
    {
        print("Damage");
        if (hitNum == 0)
        {
            hitNum++;
            sr.sprite = newState;
        }
        else if (hitNum == 1)
        {
            Destroy(gameObject);
        }
    }
}
