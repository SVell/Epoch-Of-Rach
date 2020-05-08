using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageRock : MonoBehaviour
{
    public float speed = 4f;

    private Rigidbody2D rb;
    
    public AudioSource destroy;
    private playerController player;
    private Vector3 movingDir;
    private Animator anim;

    private void Start()
    {
        try
        {
            anim = GetComponent<Animator>();
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindObjectOfType<playerController>();
            movingDir = (player.transform.position - transform.position).normalized * speed;
            rb.velocity = new Vector2(movingDir.x, movingDir.y);

            // LookAt Player.
            var dir = player.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        catch (NullReferenceException e)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GetComponent<Collider2D>().enabled = false;
            rb.velocity = Vector2.zero;
            player.TakeDamage(20);
            Destroy(gameObject,0.4f);
            anim.SetBool("Destroy",true);
            //destroy.Play();
        }
        else if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Knife"))
        {
            GetComponent<Collider2D>().enabled = false;
            rb.velocity = Vector2.zero;
            Destroy(gameObject,0.4f);
            anim.SetBool("Destroy",true);
            //destroy.Play();
        }
    }
}
