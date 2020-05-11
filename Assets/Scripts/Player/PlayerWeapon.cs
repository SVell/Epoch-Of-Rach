using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    private int knifeNumber = 0;
    
    public Transform firePoint;
    public GameObject bullet;
    public GameObject knifeUI;

    private void Start()
    {
        knifeNumber += PlayerPrefs.GetInt("knifeCount");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Shoot();
        }
    }
    
    IEnumerator PickKnife()
    {
        // TODO: Collision with key pick up
        knifeUI.SetActive(true);
        Text keyText = knifeUI.GetComponentInChildren<Text>();
        keyText.text = "x" + knifeNumber;
        yield return new WaitForSeconds(1f);
        knifeUI.SetActive(false);
    }

    public void Shoot()
    {
        if (knifeNumber > 0)
        {
            Instantiate(bullet, firePoint.position, firePoint.rotation);
            knifeNumber--;
            StartCoroutine(PickKnife());
        }
            
    }

    void OnTriggerEnter2D(Collider2D collider2D)
    {
        // Coin Pick Up
        if (collider2D.CompareTag("Knife"))
        {
            Destroy(collider2D.gameObject);
            knifeNumber++;
            StartCoroutine(PickKnife());
        }
    }
}
