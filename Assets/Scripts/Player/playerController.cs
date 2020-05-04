using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Tilemaps;
using UnityEngine.Timeline;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour
{
    #region Variables

    // Health
    public int maxHealth = 100;
    public int health;
    public HealthBar healthBar;

    // Technical
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public Animator anim;
    public float speed = 10f;
    public float time = 0;
    public playerJump plJ;
    private bool facingRight = true;

    private int coins = 0;
    private int keys = 0;

    // Attack
    public int damage = 20;
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask whatIsEnemy;
    public float damageForce = 9f;

    // UI
    public Text coinCount;
    public PauseMenu dieMenu;
    public GameObject keyUI;


    // Audio components
    public AudioSource slash;
    public AudioSource coin;
    public AudioSource walk;
    public AudioSource heal;
    public AudioSource takeDamage;

    #endregion


    #region UnityFunctions

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(Attack());
        }

        if (health <= 0)
        {
            Die();
        }

        if (Input.GetButton("Horizontal") && walk.isPlaying == false)
        {
            walk.Play();
        }
        else if (Input.GetButtonUp("Horizontal") || plJ.isGrounded == false)
        {
            walk.Stop();
        }

        coinCount.text = "x" + coins;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Vertical");
        Vector2 dir = new Vector2(x, 0);


        if (time > 0)
        {
            time--;
        }

        Walk(dir);


        if (rb.velocity.x == 0 && rb.velocity.y == 0)
        {
            anim.SetInteger("States", 0);
        }
    }

    #endregion
    
    // Attack enumerator for anim delay
    // TODO: Attack trigger delay
    IEnumerator Attack()
    {
        anim.SetInteger("States", 3);
        slash.Play();

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, whatIsEnemy);

        yield return new WaitForSeconds(0.1f);
        foreach (Collider2D x in enemies)
        {
            if (x != null && x.GetComponent<Unit>() != null)
            {
                // Damage enemies
                x.GetComponent<Unit>().TakeDamage(damage);
            }
            else if (x != null && x.GetComponent<box>() != null)
            {
                // Damage boxes
                x.GetComponent<box>().TakeDamage();
            }
        }
    }
    
    IEnumerator PickKey()
    {
        keyUI.SetActive(true);
        Text keyText = keyUI.GetComponentInChildren<Text>();
        keyText.text = "x" + keys;
        yield return new WaitForSeconds(2f);
        keyUI.SetActive(false);
    }

    private void Walk(Vector2 dir)
    {
        rb.velocity = (new Vector2(dir.x * speed, rb.velocity.y));

        if (rb.velocity.x > 0)
        {
            if(!facingRight) 
                transform.Rotate(0,180,0);
            facingRight = true;
            if (plJ.isGrounded)
            {
                anim.SetInteger("States", 1);
            }

            
            //transform.localScale = new Vector3(1, 1, 1);
        }
        else if (rb.velocity.x < 0)
        {
            if(facingRight) 
                transform.Rotate(0,180,0);
            facingRight = false;
            if (plJ.isGrounded)
            {
                anim.SetInteger("States", 1);
            }
            
            //transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        // Coin Pick Up
        if (collider2D.CompareTag("Coin"))
        {
            coin.Play();
            Destroy(collider2D.gameObject);
            coins++;
        }
        // Key pick up
        if (collider2D.CompareTag("Key"))
        {
            Destroy(collider2D.gameObject);
            keys++;
            StartCoroutine(PickKey());
        }
        // Open gate
        if (collider2D.CompareTag("Gate"))
        {
            Animator anim = collider2D.GetComponent<Animator>();
            anim.SetBool("Open",true);
            keys--;
            StartCoroutine(PickKey());
        }

        // Invisible walls
        if (collider2D.CompareTag("InvWall"))
        {
            collider2D.gameObject.GetComponent<InvWall>().Open();
        }

        // Health potions limitation
        if (health < maxHealth && collider2D.CompareTag("Potion"))
        {
            heal.Play();
            health += 20;
            if (health >= maxHealth)
                health = maxHealth;
            healthBar.SetHealth(health);
            Destroy(collider2D.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collider2D)
    {
        if (collider2D.CompareTag("InvWall"))
        {
            collider2D.gameObject.GetComponent<InvWall>().Close();
        }
    }

    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Obstacle"))
        {
            Die();
        }
    }


    public void TakeDamage(int damage)
    {
        // TODO: add force in opposite direction
        health -= damage;
        takeDamage.Play();
        healthBar.SetHealth(health);
        rb.velocity = new Vector2(rb.velocity.x, damageForce);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    void Die()
    {
        // TODO: Die sound
        takeDamage.Play();
        dieMenu.Die();
        Destroy(gameObject);
    }
}