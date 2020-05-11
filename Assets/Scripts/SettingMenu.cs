using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingMenu : MonoBehaviour
{
    public AudioSource audio;

    private float volume;
    
    // Start is called before the first frame update
    void Start()
    {
        volume = audio.volume;
    }

    // Update is called once per frame
    void Update()
    {
        AudioSource[] audios = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource aud in audios)
        {
            if(aud != audio) 
                aud.volume = 0;
        }

        //audio.volume = volume;
    }
}
