using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int max)
    {
        slider.maxValue = max;
        slider.value = max;
    }
    
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
