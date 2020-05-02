using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class WalkingEnemy : MonoBehaviour
{
    public float speed;
    public float distance = 2f;
    public int damage = 20;

    private bool movingRight = false;

    public Transform groundDetection;

    private void Update()
    {
        
        transform.Translate(Vector2.right * speed * Time.deltaTime);

        int layer_mask = LayerMask.GetMask ("Ground");
        RaycastHit2D groundInfo = Physics2D.Raycast (groundDetection.position, Vector2.down, distance, layer_mask);
        if (groundInfo.collider == false)
        {
            if (movingRight == true)
            {
                transform.eulerAngles = new UnityEngine.Vector3(0,-180,0);
                movingRight = false;
            }
            else
            {
                transform.eulerAngles = new UnityEngine.Vector3(0,0,0);
                movingRight = true;
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            collision2D.gameObject.GetComponent<playerController>().TakeDamage(damage);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            collider2D.gameObject.GetComponent<playerController>().TakeDamage(damage);
        }
    }
}
