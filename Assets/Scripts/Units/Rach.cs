using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Rach : MonoBehaviour
{
     public GameObject target;
    private bool change = false;
    private bool movingRight = true;
    private float nextFire = 0.0F;
    private float nextFireRock = 0.0F;
    private bool canShoot = true;
    
    public float spawnHeal = 20;
    public float spawnHealRate = 20f;
    
    public float spawnKnife = 20;
    public float spawnKnifeRate = 20f;
    public GameObject heal;
    public GameObject knife;

    public float fireRate = 1;
    public float fireRateRock = 5.5f;
    public GameObject player;
    public GameObject[] points;
    public GameObject fireBall;
    public GameObject stone;
    public float speed;

    public Transform spawnHealPosition;
    public Transform spawnKnifePosition;
    public GameObject[] chest;
    public GameObject portal;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("Damage") == 20)
        {
            gameObject.GetComponent<Unit>().maxHealth *= 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (StartBattle.startBattle)
        {
            Move();
            //SpawnHeal();
            //SpawnKnife();
            if (canShoot)
            {
                Shoot();
                
            }
        }
    }

    void SpawnHeal()
    {
        if (spawnHeal < 0)
        {
            Instantiate(heal, spawnHealPosition);
            spawnHeal = spawnHealRate;
        }
        else
        {
            spawnHeal -= Time.deltaTime;
        }
    }
    
    void SpawnKnife()
    {
        if (spawnKnife < 0)
        {
            Instantiate(knife, spawnHealPosition);
            spawnKnife = spawnKnifeRate;
        }
        else
        {
            spawnKnife -= Time.deltaTime;
        }
    }
    
    void Move()
    {
        if (target == null)
        {
            int num = Random.Range(0, points.Length-1);
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
            float randomValue = Random.value;
            if (randomValue < 0.7f)
            {
                Instantiate(fireBall, transform.position, Quaternion.identity);
                nextFire = Time.time + fireRate;
            }
            else
            {
                Instantiate(stone, transform.position, Quaternion.identity);
                nextFireRock = Time.time + fireRateRock;
            }
            
        }
    }
    
    void ShootRock()
    { 
        Instantiate(stone, transform.position, Quaternion.identity);
        nextFireRock = Time.time + fireRateRock;
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
            float randomValue = Random.value;
            if (randomValue < 0.5f)
            {
                speed = 6;
                change = true;
                yield return new WaitForSeconds(1f);
                target = null;
                change = false;
            }
            else if (randomValue < 0.7f)
            {
                speed = 10;
                change = true;
                yield return new WaitForSeconds(1f);
                target = null;
                change = false;
            }
            else
            {
                canShoot = false;
                change = true;
                ShootRock();
                yield return new WaitForSeconds(0.5f);
                ShootRock();
                yield return new WaitForSeconds(0.5f);
                ShootRock();
                yield return new WaitForSeconds(0.5f);
                ShootRock();
                yield return new WaitForSeconds(2f);
                target = null;
                change = false;
                canShoot = true;
            }
            
            
            
        }
        
    }
}
