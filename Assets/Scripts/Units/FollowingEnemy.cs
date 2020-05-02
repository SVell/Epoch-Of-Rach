using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class FollowingEnemy : MonoBehaviour
{

    public int damage = 20;
    public AIPath aiPath;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (aiPath.desiredVelocity.x >= 0.01f)
        {
            transform.localScale = new Vector3(-1,1,1);
        }
        else if (aiPath.desiredVelocity.x <= -0.01f)
        {
            transform.localScale = new Vector3(1,1,1);
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag == "Player")
        {
            collision2D.gameObject.GetComponent<playerController>().TakeDamage(damage);
        }
    }
}
