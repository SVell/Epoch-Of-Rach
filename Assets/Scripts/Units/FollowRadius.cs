using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class FollowRadius : MonoBehaviour
{
    public AIDestinationSetter  destination;
    public Transform startingPos;

    void Start()
    {
        // Set star pos
        destination = GetComponent<AIDestinationSetter>();
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        
        // Follow player
        if (collider2D.gameObject.tag == "Player")
        {
            destination.target = collider2D.gameObject.transform;
        }
    }
    
    void OnTriggerExit2D(Collider2D collider2D)
    {
        // Got o the starting pos
        if (collider2D.gameObject.tag == "Player")
        {
            destination.target = startingPos;
        }
    }
}
