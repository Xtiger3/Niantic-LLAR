using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DontDestroyAudio : MonoBehaviour
{
    public AudioClip[] audios;
    public static DontDestroyAudio Inst;

    private string sceneName = "";

    // Start is called before the first frame update
    void Awake()
    {

        if (Inst != null && Inst != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Inst = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    // Update is called once per frame
    //void Update()
    //{
    //    if (SceneManager.GetActiveScene().name == "argame")
    //    {
    //        this.GetComponent<AudioSource>().clip = audios[1];
    //        GetComponent<AudioSource>().Play();
    //    }
    //    else
    //    {
    //        this.GetComponent<AudioSource>().clip = audios[0];
    //        GetComponent<AudioSource>().Play();
    //    }
    //}

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name == "argame")
        {
            GetComponent<AudioSource>().clip = audios[1];
            GetComponent<AudioSource>().Play();
            sceneName = scene.name;
        }
        else
        {
            if (sceneName == "argame")
            {
                GetComponent<AudioSource>().clip = audios[0];
                GetComponent<AudioSource>().Play();
            }
            sceneName = scene.name;
        }
    }

    void Start()
    {

    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
