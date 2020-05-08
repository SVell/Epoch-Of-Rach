using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBattle : MonoBehaviour
{
    public static bool startBattle = false;

    public Unit unit;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            unit.ShowBossUI();
            startBattle = true;
        }
    }
}
