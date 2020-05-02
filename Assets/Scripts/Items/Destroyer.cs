using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
    public float timeOffset = 0.1f;

    void Start()
    {
        Invoke("Destroy",timeOffset);
    }
    
    // Update is called once per frame
    void Destroy()
    {
        Destroy(gameObject);
    }
}
