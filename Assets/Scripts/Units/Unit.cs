using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class Unit : MonoBehaviour
{
    // Parent class for all units

    public int maxHealth = 40;
    public int health = 40;
    public HealthBar healthBar;
    public SpriteRenderer sr;

    public Animator anim;

    private bool boss = false;
    
    void Start()
    {
        health = maxHealth;
        if(healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }


    public void TakeDamage(int damage)
    {
        if(sr != null)
            StartCoroutine(ChangeColor());
        health -= damage;
        if(!boss)
            StartCoroutine(ShowUI());
        if(healthBar != null)
            healthBar.SetHealth(health);
        
        if (health <= 0)
        {
            Die();
            if (boss)
            {
                BossDie();
            }
            Destroy(gameObject);
        }
    }

    IEnumerator ChangeColor()
    {
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        sr.color = Color.white;
    }
    
    IEnumerator ShowUI()
    {
        healthBar.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        healthBar.gameObject.SetActive(false);
    }

    public void ShowBossUI()
    {
        boss = true;
        anim.SetBool("Enter",true);
    }

    void Die()
    {
        // Die anim
        // Disable enemy
    }
    
    void BossDie()
    {
        for (int i = 0; i < GetComponent<BossMage>().chest.Length; i++)
        {
            GetComponent<BossMage>().chest[i].SetActive(true);
            
        }
        GetComponent<BossMage>().portal.SetActive(true);
    }
}
