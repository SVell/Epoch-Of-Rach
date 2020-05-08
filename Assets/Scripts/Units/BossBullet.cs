using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.EventSystems;

public class BossBullet : MonoBehaviour
{
    public GameObject destroyed;
    public float speed = 7f;

    private Rigidbody2D rb;
    
    public AudioSource start;
    public AudioSource destroy;
    private playerController player;
    private Vector3 movingDir;

    private void Start()
    {
        try
        {
            start.Play();
            rb = GetComponent<Rigidbody2D>();
            player = GameObject.FindObjectOfType<playerController>();
            movingDir = (player.transform.position - transform.position).normalized * speed;
            rb.velocity = new Vector2(movingDir.x, movingDir.y);

            // LookAt Player.
            var dir = player.transform.position - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
        catch (NullReferenceException)
        {
            
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player.TakeDamage(20);
            Destroy(gameObject);
            destroy.Play();
            Instantiate(destroyed, transform.position, transform.rotation);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponentInChildren<Light2D>().enabled = false;
            Destroy(gameObject,5f);
        }
        else if (other.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
            destroy.Play();
            Instantiate(destroyed, transform.position, transform.rotation);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponentInChildren<Light2D>().enabled = false;
            Destroy(gameObject,5f);
        }
        else if (other.gameObject.CompareTag("Knife"))
        {
            Destroy(gameObject);
            destroy.Play();
            Instantiate(destroyed, transform.position, transform.rotation);
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gameObject.GetComponent<Collider2D>().enabled = false;
            gameObject.GetComponentInChildren<Light2D>().enabled = false;
            Destroy(gameObject,5f);
        }
    }
}
