using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knife : MonoBehaviour
{
    public float speed = 6;
    private Rigidbody2D rb;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.gameObject.GetComponent<Unit>().TakeDamage(20);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Destroyable"))
        {
            other.gameObject.GetComponent<box>().TakeDamage();
            Destroy(gameObject);
        }

        if (!other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
