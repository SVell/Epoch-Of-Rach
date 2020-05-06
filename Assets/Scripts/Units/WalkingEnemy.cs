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
        
        transform.Translate(Vector2.right * (speed * Time.deltaTime));

        // Raycast to detect the end of the platform
        int layerMask = LayerMask.GetMask ("Ground");
        RaycastHit2D groundInfo = Physics2D.Raycast (groundDetection.position, Vector2.down, distance, layerMask);
        if (groundInfo.collider == false)
        {
            // Change direction of the movement
            if (movingRight)
            {
                //transform.eulerAngles = new UnityEngine.Vector3(0,-180,0);
                movingRight = false;
                transform.Rotate(0,180,0);
            }
            else
            {
                //transform.eulerAngles = new UnityEngine.Vector3(0,0,0);
                movingRight = true;
                transform.Rotate(0,180,0);
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.CompareTag("Player"))
        {
            collision2D.gameObject.GetComponent<playerController>().TakeDamage(damage);
        }
    }
    
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.CompareTag("Player"))
        {
            collider2D.gameObject.GetComponent<playerController>().TakeDamage(damage);
        }
    }
}
