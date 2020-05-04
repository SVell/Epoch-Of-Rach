using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public GameObject projecTile;
    public Transform point;

    public float timeOffset = 4;
    private float time;

    void Start()
    {
        time = timeOffset;
    }
    
    void Update()
    {
        // Shooting delay
        time -= Time.deltaTime;
        if (time <= 0)
        {
            Instantiate(projecTile, point.position, transform.rotation);
            time = timeOffset;
        }
    }


}
