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
        destination = GetComponent<AIDestinationSetter>();
        //startingPos = gameObject.transform;
    }

    void Update()
    {
        //if(destination.target == null)
            //destination.target = startingPos;
    }
    
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        
        if (collider2D.gameObject.tag == "Player")
        {
            destination.target = collider2D.gameObject.transform;
        }
    }
    
    void OnTriggerExit2D(Collider2D collider2D)
    {
        
        if (collider2D.gameObject.tag == "Player")
        {
            destination.target = startingPos;
        }
    }
}
