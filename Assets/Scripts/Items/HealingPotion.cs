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
        //get the objects current position and put it in a variable so we can access it later with less cod
        Vector3 pos = transform.position;
        //Vector3 pos = transform.position;
        //calculate what the new Y position will be
        float newY = Mathf.Sin(Time.time * speed) * height + pos.y;
        //set the object's Y to the new calculated Y
        transform.position = new Vector3(pos.x, newY, pos.z);
    }

}
