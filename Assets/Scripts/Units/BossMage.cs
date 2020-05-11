using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SocialPlatforms;
using Random = UnityEngine.Random;

public class BossMage : MonoBehaviour
{
    private GameObject target;
    private bool change = false;
    private bool movingRight = true;
    private float nextFire = 0.0F;
    private float nextFireRock = 0.0F;
    
    public float fireRate = 1;
    public float fireRateRock = 5.5f;
    public GameObject player;
    public GameObject[] points;
    public GameObject fireBall;
    public GameObject stone;
    public float speed;

    public GameObject[] chest;
    public GameObject portal;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (StartBattle.startBattle)
        {
            Move();
            Shoot();
            ShootRock();
            
        }
    }

   
    
    void Move()
    {
        if (target == null)
        {
            int num = Random.Range(0, 5);
            target = points[num];
        }

        if (target != null)
        {
            StartCoroutine(Stop());
        }
        
    }

    void Shoot()
    {
        if (Time.time > nextFire) {
            Instantiate(fireBall, transform.position, Quaternion.identity);
            nextFire = Time.time + fireRate;
        }
        
    }
    
    void ShootRock()
    {
        if (Time.time > nextFireRock) {
            Instantiate(stone, transform.position, Quaternion.identity);
            nextFireRock = Time.time + fireRateRock;
        }
        
    }

    IEnumerator Stop()
    {
        if (player != null && player.transform.position.x > transform.position.x && !movingRight)
        {
            transform.Rotate(0,180,0);
            movingRight = true;
        }

        if (player != null && player.transform.position.x < transform.position.x && movingRight)
        {
            transform.Rotate(0,180,0);
            movingRight = false;
        }
        
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), target.transform.position, speed * Time.deltaTime);

        
        if (target.transform.position == transform.position && change == false)
        {
            change = true;
            yield return new WaitForSeconds(1f);
            print("New Point");
            target = null;
            change = false;
        }
        
    }
}
