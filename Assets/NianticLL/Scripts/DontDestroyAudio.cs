using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyAudio : MonoBehaviour
{
    public AudioClip[] audios;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "argame")
        {
            this.GetComponent<AudioSource>().clip = audios[1];
        }
        else
        {
            this.GetComponent<AudioSource>().clip = audios[0];
        }
    }
}
