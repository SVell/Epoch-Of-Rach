using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Chest : MonoBehaviour
{
    public Animator anim;
    public GameObject[] coins = new GameObject[3];


    public AudioSource chest;
    
    
    
    void OnTriggerStay2D(Collider2D collider2D)
    {
        if (collider2D.gameObject.tag == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                chest.Play();
                anim.SetInteger("state",1);
                

                if (coins[0] != null)
                {
                    coins[0].SetActive(true);

                    StartCoroutine(CoinsSpawn());
                }
            }
        }
    }

    IEnumerator CoinsSpawn()
    {
        yield return new WaitForSeconds(0.5f);
        if (coins[1] != null)
            coins[1].SetActive(true);
        yield return new WaitForSeconds(0.5f);
        if (coins[2] != null)
            coins[2].SetActive(true);
    }
}
