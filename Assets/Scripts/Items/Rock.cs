using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    private Animator anim;

    public bool left;
    
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        if (left)
        {
            transform.eulerAngles = new UnityEngine.Vector3(0,-180,0);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            anim.SetBool("Destroy",true);
        }
    }
}
