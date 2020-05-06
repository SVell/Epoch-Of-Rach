using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    private bool open = false;
    
    public Transform point1;
    public float speed = 6;

    private void Update()
    {
        if(open)
            transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), point1.transform.position, speed * Time.deltaTime);
    }


    public void Open()
    {
        open = true;
    }
}
