using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
    public Rach boss;

    private Win win;
    // Start is called before the first frame update
    void Start()
    {
        boss = FindObjectOfType<Rach>();
        win = FindObjectOfType<Win>();
    }

    // Update is called once per frame
    void Update()
    {
        if (boss == null)
            StartCoroutine(win.Load());
    }
}
