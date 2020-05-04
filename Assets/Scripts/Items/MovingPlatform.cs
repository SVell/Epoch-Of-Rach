using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private bool isMovingUP = true;
    
    public float speed = 3;
    public GameObject point1;
    public GameObject point2;
    public Transform parent;


    // Update is called once per frame
    void FixedUpdate()
    {
        if(isMovingUP)
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), point1.transform.position, speed * Time.deltaTime);
        else
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), point2.transform.position, speed * Time.deltaTime);

        if (transform.position == point1.transform.position)
            isMovingUP = false;
        else if(transform.position == point2.transform.position)
            isMovingUP = true;
    }
    
    void OnCollisionEnter2D(Collision2D other) {
        if (other.transform.tag == "Player") {
            other.transform.parent = transform;
        }
    }
 
    private void OnCollisionExit2D(Collision2D other) {
        if (other.transform.tag == "Player") {
            other.transform.parent = parent;
        }
    }
}
