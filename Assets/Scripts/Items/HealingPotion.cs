using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class HealingPotion : MonoBehaviour
{
    float speed = 3f;
//adjust this to change how high it goes
    float height = 0.002f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Move up and down
        Vector3 pos = transform.position;
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        transform.position = new Vector3(pos.x, newY, pos.z);
    }

}
