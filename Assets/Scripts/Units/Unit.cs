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

    public GameObject coins;
    
    public Animator anim;

    public bool boss = false;
    public bool rach = false;

    public AudioSource hit;

    public GameObject end;
    
    void Start()
    {
        if (boss && PlayerPrefs.GetInt("damage") == 20)
        {
            maxHealth *= 2;
            health = maxHealth;
        }
        else if (rach && PlayerPrefs.GetInt("damage") == 20)
        {
            maxHealth *= 2;
            health = maxHealth;
        }
        else
        {
            health = maxHealth;
        }
        
        if(healthBar != null)
            healthBar.SetMaxHealth(maxHealth);
    }


    public void TakeDamage(int damage)
    {
        if(hit!=null)
            hit.Play();
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

            if (end != null)
            {
                end.SetActive(true);
                if (rach)
                {
                    Time.timeScale = 0f;
                }
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
        if(coins != null)
            Instantiate(coins,transform.position,transform.rotation);
    }
    
    void BossDie()
    {
        if (GetComponent<BossMage>())
        {
            for (int i = 0; i < GetComponent<BossMage>().chest.Length; i++)
            {
                GetComponent<BossMage>().chest[i].SetActive(true);
            
            }
            GetComponent<BossMage>().portal.SetActive(true);
        }
    }
}
