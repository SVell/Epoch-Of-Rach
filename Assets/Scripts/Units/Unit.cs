using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;

    void Start()
    {
        health = maxHealth;
    }
    
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        
        if (health <= 0)
        {
            Die();
            Destroy(gameObject);
        }
    }

    void Die()
    {
        // Die anim
        // Disable enemy
    }
}
